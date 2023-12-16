using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchedBus.Models;
using SchedBus.Services;
using System.Collections.ObjectModel;

namespace SchedBus.ViewModels;

public partial class PlanEditViewModel : ObservableObject, IQueryAttributable
{
    protected static PlanSQLiteService Database => PlanSQLiteService.Instance;
    protected static GoogleMapsApiService GoogleMapsApi => GoogleMapsApiService.Instance;

    [ObservableProperty]
    public ObservableCollection<GooglePlacesApi.Place> googlePlaces;

    [ObservableProperty]
    public Plan plan;

    public PlanEditViewModel()
    {
        Plan = new Plan();
        GooglePlaces = new ObservableCollection<GooglePlacesApi.Place>();
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("selectedPlan"))
        {
            Plan = query["selectedPlan"] as Plan;
            query.Clear();
        }
        else if (query.ContainsKey("saveTimeset"))
        {
            int index = 0;
            var timeset = query["saveTimeset"] as TimeSet;
            query.Clear();
            if (timeset.Id != 0)
            {
                foreach (var item in Plan.TimeSets)
                {
                    if (item.Id == timeset.Id)
                    {
                        index = Plan.TimeSets.IndexOf(item);
                        break;
                    }
                }
                Plan.TimeSets[index] = timeset;
                OnPropertyChanged(nameof(Plan));
            }
            else
            {
                if (Plan.TimeSets.Any(i => i.Time == timeset.Time))
                {
                    Console.WriteLine("Duplicated Time! Cannot Save");
                }
                else
                {
                    Plan.TimeSets.Add(timeset);
                }
                OnPropertyChanged(nameof(Plan));
            }
        }
        else if (query.ContainsKey("deleteTimeset"))
        {
            var timeset = query["deleteTimeset"] as TimeSet;
            query.Clear();
            if (timeset.Id != 0)
            {
                if (Plan.TimeSets.Count > 0)
                {
                    foreach (var item in Plan.TimeSets)
                    {
                        if (item.Id == timeset.Id)
                        {
                            Plan.TimeSets.Remove(item);
                            OnPropertyChanged(nameof(Plan));
                            break;
                        }
                    }
                }
            }
        }
        else if (query.ContainsKey("destination"))
        {
            Plan.Destination = query["destination"] as Destination;
            query.Clear();
            OnPropertyChanged(nameof(Plan));
        }
    }

    [RelayCommand]
    public async Task GetPlaces(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            GooglePlaces.Clear();
        }
        else
        {
            GooglePlaces = await GoogleMapsApi.RequestPlaces(text);
        }
    }

    [RelayCommand]
    public void SelectPlace(GooglePlacesApi.Place place)
    {
        Plan.Destination.Name = place.displayName.text;
        Plan.Destination.Address = place.formattedAddress;
        Plan.Destination.Latitude = place.location.latitude;
        Plan.Destination.Longitude = place.location.longitude;

        OnPropertyChanged(nameof(Plan));

        GooglePlaces.Clear();
    }

    [RelayCommand]
    public async Task SearchByMap()
    {
        await Shell.Current.GoToAsync($"{nameof(Pages.LocationSearch)}");
    }

    [RelayCommand]
    public async Task AddTimeSet()
    {
        await Shell.Current.GoToAsync($"{nameof(Pages.TimeSetPage)}");
    }

    [RelayCommand]
    public async Task SelectTimeset(TimeSet timset)
    {
        var navigationParameter = new Dictionary<string, object>() { { "selectedTimeset", timset } };
        await Shell.Current.GoToAsync($"{nameof(Pages.TimeSetPage)}", navigationParameter);
    }

    [RelayCommand]
    public async Task SavePlan()
    {
        if (Plan.Destination != null)
        {
            await Database.SavePlanAsync(Plan);
            await Shell.Current.GoToAsync($"..");
        }
        else
        {
            await Shell.Current.GoToAsync($"..");
        }
    }

    [RelayCommand]
    public async Task DeletePlan()
    {
        if( Plan.Id != 0 )
        {
            await Database.DeletePlanAsync(Plan);
            await Shell.Current.GoToAsync($"..");
        }
        else
        {
            await Shell.Current.GoToAsync($"..");
        }
    }
}
