using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchedBus.Models;
using System.Collections.ObjectModel;

namespace SchedBus.ViewModels;

public class TimeSetViewModel : ObservableObject, IQueryAttributable
{
    public TimeSet TimeSet { get; set; }

    public ObservableCollection<string> HourItemsSource { get; set; }
    public ObservableCollection<string> MinuteItemsSource { get; set; }
    public ObservableCollection<string> AMPMItemsSource { get; set; }

    public IAsyncRelayCommand SaveCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }

    public TimeSetViewModel()
    {
        TimeSet = new TimeSet();
        HourItemsSource = new ObservableCollection<string>(Enumerable.Range(1, 12).Select(h => h.ToString()));
        MinuteItemsSource = new ObservableCollection<string>(Enumerable.Range(0, 60).Select(m => m.ToString("00")));
        AMPMItemsSource = new ObservableCollection<string>(new[] { "AM", "PM" });
        SaveCommand = new AsyncRelayCommand(() => Save(SelectedTimeSet));
        DeleteCommand = new AsyncRelayCommand(() => Delete(SelectedTimeSet));
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("selectedtimeset"))
        {
            TimeSet = query["selectedtimeset"] as TimeSet;

            OnPropertyChanged(nameof(TimeSet));
        }
    }

    public TimeSet SelectedTimeSet
    {
        get => TimeSet;
        set
        {
            TimeSet = value;
            OnPropertyChanged(nameof(SelectedTimeSet));
        }
    }
    public async Task Save(TimeSet timeSet)
    {
        //var timeSetId = await SqliteDataStore.SaveTimeSetAsync(_timeset, _planId);
        await Shell.Current.GoToAsync($"..");
    }

    public async Task Delete(TimeSet timeSet)
    {
        //await SqliteDataStore.RemoveNotUsedTimeSetAsync(_timeset.Id, _planId);
        await Shell.Current.GoToAsync($"..");
    }
}
