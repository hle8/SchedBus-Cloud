using System.Collections.ObjectModel;

namespace SchedBus.Models;

public class GooglePlace
{
    public string? formattedName { get; set; }
    public string? formattedAddress { get; set; }
    public Dictionary<string, double>? location { get; set; }
    public Dictionary<string, string>? displayName { get; set; }
}

public class GooglePlaces
{
    public ObservableCollection<GooglePlace>? places { get; set; }
}

public class GooglePlaceQuery
{
    public string? textQuery { get; set; }
}