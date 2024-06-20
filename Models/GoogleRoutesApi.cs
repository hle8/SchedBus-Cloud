namespace SchedBus.Models;

public class GoogleRoutesApi
{
    public class LatLng
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Location
    {
        public LatLng? latLng { get; set; }
    }

    public class Stop
    {
        public string? name { get; set; }
        public Location? location { get; set; }
    }

    public class Time
    {
        public string? text { get; set; }
    }

    public class ArrDepTime
    {
        public Time? time { get; set; }
        public string? timeZone { get; set; }
    }

    public class LocalizedValues
    {
        public ArrDepTime? arrivalTime { get; set; }
        public ArrDepTime? departureTime { get; set; }
    }

    public class Agency
    {
        public string? name { get; set; }
        public string? phoneNumber { get; set; }
        public string? uri { get; set; }
    }

    public class Name
    {
        public string? text;
    }

    public class Vehicle
    {
        public Name? name { get; set; }
        public string? type { get; set; }
        public string? iconUri { get; set; }
    }

    public class TransitLine
    {
        public List<Agency>? agencies { get; set; }
        public string? name { get; set; }
        public string? color { get; set; }
        public string? nameShort { get; set; }
        public string? textColor { get; set; }
        public Vehicle? vehicle { get; set; }
    }

    public class StopDetails
    {
        public Stop? arrivalStop { get; set; }
        public string? arrivalTime { get; set; }
        public Stop? departureStop { get; set; }
        public string? departureTime { get; set; }
    }

    public class TransitDetails
    {
        public StopDetails? stopDetails { get; set; }
        public LocalizedValues? localizedValues { get; set; }
        public string? headsign { get; set; }
        public TransitLine? transitLine { get; set; }
        public int stopCount { get; set; }
    }

    public class Step
    {
        public TransitDetails? transitDetails { get; set; }
    }

    public class Leg
    {
        public List<Step>? steps { get; set; }
    }

    public class Route
    {
        public List<Leg>? legs { get; set; }
    }

    public class Root
    {
        public List<Route>? routes { get; set; }
    }

    public class InputPlace
    {
        public Location? location { get; set; }
    }

    public class TransitPreferences
    {
        public string? routingPreference { get; set; }
        public string[]? allowedTravelModes { get; set; }
    }

    public class RouteQurery
    {
        public InputPlace? origin { get; set; }
        public InputPlace? destination { get; set; }
        public string? travelMode { get; set; }
        public bool computeAlternativeRoutes { get; set; }
        public string? arrivalTime { get; set; }
        public string? departureTime { get; set; }
        public TransitPreferences transitPreferences { get; set; }
    }

}
