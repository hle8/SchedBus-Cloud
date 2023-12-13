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
    ObservableCollection<Place> googlePlaces;

    [ObservableProperty]
    string searchText;

    public LocationSearchViewModel()
    {
        GooglePlaces = [];
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
    public void SelectPlace(Place place)
    {
        SearchText = place.displayName.text;
        GooglePlaces.Clear();
    }

    [RelayCommand]
    public async Task Save()
    {
        if (string.IsNullOrEmpty(SearchText))
        {
            Console.WriteLine("Alert", "Destination cannot be empty!", "OK");
            return;
        }
        await Shell.Current.GoToAsync($"..");
    }

    [RelayCommand]
    public async Task Delete()
    {
        await Shell.Current.GoToAsync($"..");
    }
}
