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
        if (TimeSet != null) 
        {
            var navigationParameter = new Dictionary<string, object> { { "saveTimeset", TimeSet } };
            await Shell.Current.GoToAsync($"..", navigationParameter);
        }
        else
        {
            await Shell.Current.GoToAsync($"..");
        }
    }

    [RelayCommand]
    public async Task DeleteTimeset()
    {
        if (TimeSet != null) 
        {
            var navigationParameter = new Dictionary<string, object> { { "deleteTimeset", TimeSet } };
            await Shell.Current.GoToAsync($"..", navigationParameter);
        }
        else
        {
            await Shell.Current.GoToAsync($"..");
        }
    }
}
