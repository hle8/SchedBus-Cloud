using System.Collections.ObjectModel;

namespace SchedBus.Models;

public class GooglePlacesApi
{
    public class DisplayName
    {
        public string? text { set; get; }
        public string? languageCode { set; get; }
    }

    public class LatLng
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Place
    {
        public string? formattedAddress { get; set; }
        public LatLng? location { get; set; }
        public DisplayName? displayName { get; set; }
    }

    public class Places
    {
        public ObservableCollection<Place>? places { get; set; }
    }

    public class PlaceQuery
    {
        public string? textQuery { get; set; }
    }
}
