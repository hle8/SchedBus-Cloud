using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchedBus.Extensions;
using SchedBus.Models;
using SchedBus.Models.FirestoreDocuments;
using SchedBus.Services;

namespace SchedBus.ViewModels;

public partial class PlanEditViewModel : ObservableObject, IQueryAttributable
{
    protected static PlanSQLiteService Database => PlanSQLiteService.Instance;
    protected static GoogleMapsApiService GoogleMapsApi => GoogleMapsApiService.Instance;

    private readonly FirestoreService _firestoreService;
    private readonly PlansViewModel _plansViewModel;

    [ObservableProperty]
    private ObservableCollection<GooglePlacesApi.Place>? googlePlaces;

    [ObservableProperty]
    private Plan plan = new();

    [ObservableProperty]
    private PlanDocument planDocument = new();

    public PlanEditViewModel(FirestoreService firestoreService, PlansViewModel plansViewModel)
    {
        _firestoreService = firestoreService;
        _plansViewModel = plansViewModel;
        GooglePlaces = [];
    }

    [RelayCommand]
    public async Task GetPlaces(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            GooglePlaces?.Clear();
        }
        else
        {
            GooglePlaces = await GoogleMapsApi.RequestPlaces(text);
        }
    }

    [RelayCommand]
    public void SelectPlace(GooglePlacesApi.Place place)
    {
        PlanDocument.Destination = place.formattedAddress;

        OnPropertyChanged(nameof(Plan));

        GooglePlaces?.Clear();
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
        var navigationParameter = new Dictionary<string, object>()
        {
            { "selectedTimeset", timset }
        };
        await Shell.Current.GoToAsync($"{nameof(Pages.TimeSetPage)}", navigationParameter);
    }

    [RelayCommand]
    public async Task SavePlan()
    {
        OnPropertyChanged(nameof(PlanDocument));

        if (PlanDocument.Destination != null)
        {
            if (PlanDocument.Id == null)
            {
                await _firestoreService.AddPlan(PlanDocument, _plansViewModel.User.GetUserEmail());
                _plansViewModel.RefreshPlans();
            }
            else
            {
                await _firestoreService.UpdatePlan(PlanDocument, _plansViewModel.User.GetUserEmail());
                _plansViewModel.RefreshPlans();
            }

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
        if (PlanDocument.Id != null)
        {
            await _firestoreService.DeletePlan(PlanDocument.Id, _plansViewModel.User.GetUserEmail());
            _plansViewModel.RefreshPlans();
            await Shell.Current.GoToAsync($"..");
        }
        else
        {
            await Shell.Current.GoToAsync($"..");
        }
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("selectedPlan", out object? value))
        {
            PlanDocument = (PlanDocument)value;
            query.Clear();
        }
    }
}
