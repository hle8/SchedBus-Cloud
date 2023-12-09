using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchedBus.Models;

namespace SchedBus.ViewModels;

internal class TimeSetViewModel : ObservableObject, IQueryAttributable
{
    public TimeSet TimeSet { get; set; }

    public IAsyncRelayCommand SaveCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }

    public TimeSetViewModel()
    {
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("selectedtimeset"))
        {
            TimeSet = query["selectedtimeset"] as TimeSet;

            OnPropertyChanged(nameof(TimeSet));
        }
    }

    public async Task Save()
    {
        //var timeSetId = await SqliteDataStore.SaveTimeSetAsync(_timeset, _planId);
        await Shell.Current.GoToAsync($"..");
    }

    public async Task Delete()
    {
        //await SqliteDataStore.RemoveNotUsedTimeSetAsync(_timeset.Id, _planId);
        await Shell.Current.GoToAsync($"..");
    }
}
