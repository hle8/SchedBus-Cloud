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

    [ObservableProperty]
    ObservableCollection<Destination> destinations;

    Destination Destination { get; set; }

    public LocationSearchViewModel()
    {
        GooglePlaces = new ObservableCollection<GooglePlacesApi.Place>();
        Destinations = new ObservableCollection<Destination>();
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

        Destinations.Clear();

        Destination.Name = place.displayName.text;
        Destination.Address = place.formattedAddress;
        Destination.Latitude = place.location.latitude;
        Destination.Longitude = place.location.longitude;
        Destination.Location = new Location(place.location.latitude, place.location.longitude);

        Destinations.Add(Destination);

        GooglePlaces.Clear();
    }

    [RelayCommand]
    public async Task Save()
    {
        if (Destination == null)
        {
            Console.WriteLine("Alert", "Destination cannot be empty!");
            return;
        }
        else
        {
            var navigationParameter = new Dictionary<string, object> { { "destination", Destinations[0] } };
            await Shell.Current.GoToAsync($"..", navigationParameter);
        }
    }

    [RelayCommand]
    public async Task Back()
    {
        await Shell.Current.GoToAsync($"..");
    }
}
