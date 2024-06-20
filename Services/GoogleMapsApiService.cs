using SchedBus.Models;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;

namespace SchedBus.Services;

public class GoogleMapsApiService
{
    static GoogleMapsApiService? _instance;

    // Singleton instance property
    public static GoogleMapsApiService Instance => _instance ??= new GoogleMapsApiService();

    HttpClient _client;

    readonly string _placesApiUrl;
    readonly string _routesApiUrl;

    readonly string _apiKey;

    // Private constructor for singleton pattern
    private GoogleMapsApiService()
    {
        _client = new HttpClient();
        _apiKey = "your-api-key";
        _placesApiUrl = "https://places.googleapis.com/v1/places:searchText";
        _routesApiUrl = "https://routes.googleapis.com/directions/v2:computeRoutes";
    }

    static bool IsConnected() => Connectivity.Current.NetworkAccess != NetworkAccess.None;

    public async Task<ObservableCollection<GooglePlacesApi.Place>?> RequestPlaces(string text)
    {
        if (!IsConnected()) return null;

        var query = new GooglePlacesApi.PlaceQuery
        {
            textQuery = text
        };

        var jsonString = JsonSerializer.Serialize(query);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_placesApiUrl),
            Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
        };

        request.Headers.Add("X-Goog-Api-Key", _apiKey);
        request.Headers.Add("X-Goog-FieldMask", "places.displayName,places.formattedAddress,places.location");

        try
        {
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<GooglePlacesApi.Places>(responseBody);

            return result?.places;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<ObservableCollection<GoogleRoutesApi.Route>?> RequestRoutes(GoogleRoutesApi.InputPlace origin, GoogleRoutesApi.InputPlace destination)
    {
        if (!IsConnected()) return null;

        var query = new GoogleRoutesApi.RouteQurery
        {
            origin = origin,
            destination = destination,
            travelMode = "TRANSIT",
            computeAlternativeRoutes = true,
            transitPreferences = new GoogleRoutesApi.TransitPreferences
            {
                routingPreference = "LESS_WALKING",
                allowedTravelModes = ["BUS"]
            }
        };

        var jsonString = JsonSerializer.Serialize(query);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_routesApiUrl),
            Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
        };

        request.Headers.Add("X-Goog-Api-Key", _apiKey);
        request.Headers.Add("X-Goog-FieldMask", "routes.legs.steps.transitDetails");

        try
        {
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<GoogleRoutesApi.Root>(responseBody);

            return new ObservableCollection<GoogleRoutesApi.Route>(result.routes);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}