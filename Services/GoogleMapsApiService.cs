using SchedBus.Models;
using System.Text;
using System.Text.Json;

namespace SchedBus.Services;

public class GoogleMapsApiService
{
    private static GoogleMapsApiService _instance;

    // Singleton instance property
    public static GoogleMapsApiService Instance => _instance ??= new GoogleMapsApiService();

    private HttpClient _client;

    private readonly string _baseUrl;

    private readonly string _apiKey;

    // Private constructor for singleton pattern
    private GoogleMapsApiService()
    {
        _client = new HttpClient();
        _apiKey = "AIzaSyBTZXqc28-40aOTkE_rDUFAo_r0ezidXXg";
        _baseUrl = "https://places.googleapis.com/v1/places:searchText"; // Set the base URL of your API here
    }

    private bool IsConnected()
    {
        return Connectivity.Current.NetworkAccess != NetworkAccess.None;
    }

    public async Task<GooglePlaces> RequestPlaces(string text)
    {
        if (!IsConnected()) { return null; }

        var query = new GooglePlaceQuery
        {
            textQuery = text
        };

        string jsonString = JsonSerializer.Serialize(query);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_baseUrl),
            Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
        };

        request.Headers.Add("X-Goog-Api-Key", _apiKey);
        request.Headers.Add("X-Goog-FieldMask", "places.displayName,places.formattedAddress");

        try
        {
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            GooglePlaces result = JsonSerializer.Deserialize<GooglePlaces>(responseBody);
            foreach (var place in result.places)
            {
                place.formattedName = place.displayName["text"];
            }

            return result;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

}
