using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchedBus.Models;
using System.Windows.Input;

namespace SchedBus.ViewModels;

internal class TimeSetViewModel : ObservableObject
{
    private TimeSet _timeset;
    public TimeSpan SetTime { get => _timeset.SetTime; set => _timeset.SetTime = value; }
    public bool IsEnabled { get => _timeset.IsEnabled; set => _timeset.IsEnabled = value; }
    public bool RepeatedOnMonday { get => _timeset.RepeatedOnMonday; set => _timeset.RepeatedOnMonday = value; }
    public bool RepeatedOnTuesday { get => _timeset.RepeatedOnTuesday; set => _timeset.RepeatedOnTuesday = value; }
    public bool RepeatedOnWednesday { get => _timeset.RepeatedOnWednesday; set => _timeset.RepeatedOnWednesday = value; }
    public bool RepeatedOnThursday { get => _timeset.RepeatedOnThursday; set => _timeset.RepeatedOnThursday = value; }
    public bool RepeatedOnFriday { get => _timeset.RepeatedOnFriday; set => _timeset.RepeatedOnFriday = value; }
    public bool RepeatedOnSaturday { get => _timeset.RepeatedOnSaturday; set => _timeset.RepeatedOnSaturday = value; }
    public bool RepeatedOnSunday { get => _timeset.RepeatedOnSunday; set => _timeset.RepeatedOnSunday = value; }

    public ICommand SaveCommand { set; get; }
    public ICommand DeleteCommand { set; get; }

    public TimeSetViewModel()
    {
        _timeset = new();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);

        OnPropertyChanged(nameof(SetTime));
        OnPropertyChanged(nameof(IsEnabled));
        OnPropertyChanged(nameof(RepeatedOnMonday));
        OnPropertyChanged(nameof(RepeatedOnTuesday));
        OnPropertyChanged(nameof(RepeatedOnWednesday));
        OnPropertyChanged(nameof(RepeatedOnThursday));
        OnPropertyChanged(nameof(RepeatedOnFriday));
        OnPropertyChanged(nameof(RepeatedOnSaturday));
        OnPropertyChanged(nameof(RepeatedOnSunday));
    }

    public TimeSetViewModel(TimeSet timeset) => _timeset = timeset;

    public async Task Save()
    {
        await Shell.Current.GoToAsync($"..");
    }

    public async Task Delete()
    {
        await Shell.Current.GoToAsync($"..");
    }
}
