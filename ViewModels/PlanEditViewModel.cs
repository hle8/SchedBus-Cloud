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
    public ObservableCollection<Place> googlePlaces;

    [ObservableProperty]
    public Plan plan;

    public PlanEditViewModel()
    {
        Plan = new Plan();
        GooglePlaces = new ObservableCollection<Place>();
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("selectedPlan"))
        {
            Plan = query["selectedPlan"] as Plan;

            OnPropertyChanged(nameof(Plan));
        }
        else if (query.ContainsKey("saveTimeset"))
        {
            var timeset = query["saveTimeset"] as TimeSet;
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
            var result = await GoogleMapsApi.RequestPlaces(text);
            GooglePlaces = result;
        }
    }

    [RelayCommand]
    public async Task SelectPlace(Place place)
    {
        Plan.Destination.Name = place.displayName.text;
        Plan.Destination.Address = place.formattedAddress;
        Plan.Destination.Latitude = place.location.latitude;
        Plan.Destination.Longitude = place.location.longitude;

        GooglePlaces.Clear();
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
        await Database.SavePlanAsync(Plan);
        await Shell.Current.GoToAsync($"..");
    }

    [RelayCommand]
    public async Task DeletePlan()
    {
        await Database.DeletePlanAsync(Plan);
        await Shell.Current.GoToAsync($"..");
    }
}
