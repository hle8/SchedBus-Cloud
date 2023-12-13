using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchedBus.Models;

namespace SchedBus.ViewModels;

public partial class TimeSetViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    public TimeSet timeSet;

    public TimeSetViewModel()
    {
        TimeSet = new TimeSet();
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("selectedTimeset"))
        {
            TimeSet = query["selectedTimeset"] as TimeSet;
        }
    }

    [RelayCommand]
    public async Task SaveTimeset()
    {
        await Shell.Current.GoToAsync($"..");
    }

    [RelayCommand]
    public async Task DeleteTimeset()
    {
        await Shell.Current.GoToAsync($"..");
    }
}
