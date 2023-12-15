using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchedBus.Models;
using SchedBus.Services;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace SchedBus.ViewModels;

public partial class LocationSearchViewModel : ObservableObject
{
    protected static PlanSQLiteService Database => PlanSQLiteService.Instance;
    protected static GoogleMapsApiService GoogleMapsApi => GoogleMapsApiService.Instance;

    [ObservableProperty]
    ObservableCollection<GooglePlacesApi.Place> googlePlaces;

    [ObservableProperty]
    string? searchResult;

    Destination Destination { get; set; }

    public LocationSearchViewModel()
    {
        GooglePlaces = new ObservableCollection<GooglePlacesApi.Place>();
        Destination = new Destination();
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
        SearchResult = place.displayName.text;

        Destination.Name = place.displayName.text;
        Destination.Address = place.formattedAddress;
        Destination.Latitude = place.location.latitude;
        Destination.Longitude = place.location.longitude;

        GooglePlaces.Clear();
    }

    [RelayCommand]
    public async Task Save()
    {
        if (string.IsNullOrEmpty(SearchResult))
        {
            Console.WriteLine("Alert", "Destination cannot be empty!");
            return;
        }
        else
        {
            var navigationParameter = new Dictionary<string, object> { { "destination", Destination } };
            await Shell.Current.GoToAsync($"..", navigationParameter);
        }
    }

    [RelayCommand]
    public async Task Back()
    {
        await Shell.Current.GoToAsync($"..");
    }
}
