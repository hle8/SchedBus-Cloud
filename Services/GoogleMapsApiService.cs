using SchedBus.Models;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using static SchedBus.Models.GoogleRoutesApi;

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
        _apiKey = "AIzaSyBTZXqc28-40aOTkE_rDUFAo_r0ezidXXg";
        _placesApiUrl = "https://places.googleapis.com/v1/places:searchText";
        _routesApiUrl = "https://routes.googleapis.com/directions/v2:computeRoutes";
    }

    static bool IsConnected() => Connectivity.Current.NetworkAccess != NetworkAccess.None;

    public async Task<ObservableCollection<Place>?> RequestPlaces(string text)
    {
        if (!IsConnected()) return null;

        var query = new PlaceQuery
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
        request.Headers.Add("X-Goog-FieldMask", "places.displayName,places.formattedAddress");

        try
        {
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<Places>(responseBody);

            return result?.places;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<List<Leg>?> RequestRoutes(string text)
    {
        if (!IsConnected()) return null;

        var query = new PlaceQuery
        {
            textQuery = text
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

            var result = JsonSerializer.Deserialize<Route>(responseBody);

            return result?.legs;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}