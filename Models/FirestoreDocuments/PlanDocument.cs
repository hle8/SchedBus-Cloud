using CommunityToolkit.Mvvm.ComponentModel;
using Google.Cloud.Firestore;

namespace SchedBus.Models.FirestoreDocuments;

[FirestoreData]
public partial class PlanDocument : ObservableObject
{
    public string? Id { get; set; }

    [ObservableProperty]
    [property: FirestoreProperty]
    private string? destination;

    [ObservableProperty]
    [property: FirestoreProperty]
    private string? label;

    [ObservableProperty]
    [property: FirestoreProperty]
    private int maxNumberOfRoutes;

    [ObservableProperty]
    [property: FirestoreProperty]
    private bool notification;

    [ObservableProperty]
    [property: FirestoreProperty]
    private bool vibration;
}
