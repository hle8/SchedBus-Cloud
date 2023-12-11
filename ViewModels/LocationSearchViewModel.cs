using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchedBus.Models;
using SchedBus.Services;
using System.Collections.ObjectModel;

namespace SchedBus.ViewModels;

public partial class LocationSearchViewModel : ObservableObject
{
    protected static PlanSQLiteService Database => PlanSQLiteService.Instance;
    protected static GoogleMapsApiService GoogleMapsApi => GoogleMapsApiService.Instance;

    [ObservableProperty]
    ObservableCollection<GooglePlace> googlePlaces;

    [ObservableProperty]
    string searchText;

    public LocationSearchViewModel() { GooglePlaces = []; }

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
            GooglePlaces = result.places;
        }
    }

    [RelayCommand]
    public void SelectPlace(GooglePlace place)
    {
        SearchText = place.displayName["text"];
        GooglePlaces.Clear();
    }
}
