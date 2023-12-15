using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchedBus.Models;
using SchedBus.Services;
using System.Collections.ObjectModel;

namespace SchedBus.ViewModels;

public partial class HomePageViewModel : ObservableObject
{
    protected static PlanSQLiteService Database => PlanSQLiteService.Instance;
    protected static GoogleMapsApiService GoogleMapsApi => GoogleMapsApiService.Instance;

    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;

    [ObservableProperty]
    ObservableCollection<GooglePlacesApi.Place> googlePlaces;
    
    [ObservableProperty]
    ObservableCollection<GoogleRoutesApi.Route> googleRoutes;

    [ObservableProperty]
    ObservableCollection<Plan> plans;

    [ObservableProperty]
    string? searchResult;

    [ObservableProperty]
    bool isPlansViewEnabled;

    [ObservableProperty]
    bool isRoutesViewEnabled;

    Destination Destination { get; set; }

    Location location { get; set; }

    public HomePageViewModel()
    {
        IsPlansViewEnabled = true;
        IsRoutesViewEnabled = false;
        GooglePlaces = new ObservableCollection<GooglePlacesApi.Place>();
        Plans = new ObservableCollection<Plan>();
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

        GooglePlaces.Clear();

        GetRoutes(place.formattedAddress, place.location.latitude, place.location.longitude);
    }

    [RelayCommand]
    public async Task GetPlans()
    {
        Plans = await Database.GetPlansAsync();
    }

    [RelayCommand]
    public async Task SelectPlan(Plan plan)
    {
        GetRoutes(plan.Destination.Address, plan.Destination.Latitude, plan.Destination.Longitude);
    }

    public async void GetRoutes(string address, double latitude, double longitude)
    {
        await GetCurrentLocation();
        GoogleRoutesApi.InputPlace origin = new GoogleRoutesApi.InputPlace
        {
            location = new GoogleRoutesApi.Location
            {
                latLng = new GoogleRoutesApi.LatLng
                {
                    latitude = location.Latitude, longitude = location.Longitude
                }
            }
        };
        GoogleRoutesApi.InputPlace destination = new GoogleRoutesApi.InputPlace
        {
            location = new GoogleRoutesApi.Location
            {
                latLng = new GoogleRoutesApi.LatLng
                {
                    latitude = latitude,
                    longitude = longitude
                }
            }
        };
        GoogleRoutes = await GoogleMapsApi.RequestRoutes(origin, destination);

        GoogleRoutes = GoogleRoutes.Where(i => i.legs != null).ToObservableCollection();
        foreach (var route in GoogleRoutes)
        {
            foreach (var leg in route.legs)
            {
                leg.steps = leg.steps.Where(i=>i.transitDetails != null).ToList();
            }
        }

        IsPlansViewEnabled = false;
        IsRoutesViewEnabled = true;
    }

    [RelayCommand]
    public void CloseRouteList()
    {
        IsPlansViewEnabled = true;
        IsRoutesViewEnabled = false;
        GoogleRoutes.Clear();
    }

    public async Task GetCurrentLocation()
    {
        try
        {
            _isCheckingLocation = true;

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            _cancelTokenSource = new CancellationTokenSource();

            location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
        }
        // Catch one of the following exceptions:
        //   FeatureNotSupportedException
        //   FeatureNotEnabledException
        //   PermissionException
        catch (Exception ex)
        {
            // Unable to get location
        }
        finally
        {
            _isCheckingLocation = false;
        }
    }

    public void CancelRequest()
    {
        if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
            _cancelTokenSource.Cancel();
    }
}
