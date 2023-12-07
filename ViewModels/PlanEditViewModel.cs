using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchedBus.Models;
using SchedBus.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SchedBus.ViewModels;

internal class PlanEditViewModel : ObservableObject, IQueryAttributable
{
    Plan _plan;

    public ObservableCollection<TimeSetViewModel> TimeSet { get; set; }
    public int Id { get => _plan.Id; set => _plan.Id = value; }
    public string Label { get => _plan.Label; set => _plan.Label = value; }
    public int MaxNumberOfRoutes { get => _plan.MaxNumberOfRoutes; set => _plan.MaxNumberOfRoutes = value; }
    public bool Vibration { get => _plan.Vibration; set => _plan.Vibration = value; }
    public bool Notification { get => _plan.Notification; set => _plan.Notification = value; }

    public IDataStore SqliteDataStore => DependencyService.Get<IDataStore>();

    public ICommand SaveCommand { set; get; }
    public ICommand DeleteCommand { set; get; }

    public PlanEditViewModel()
    {
        _plan = new Plan();
        TimeSet = new ObservableCollection<TimeSetViewModel>();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public PlanEditViewModel(Plan plan) => _plan = plan;

    async void IQueryAttributable.ApplyQueryAttributes(System.Collections.Generic.IDictionary<string, object> query)
    {
        if (!query.ContainsKey("id")) return;

        var id = Convert.ToInt32(query["id"].ToString);
        _plan = await SqliteDataStore.GetPlanAsync(id);

        OnPropertyChanged(nameof(Id));
        OnPropertyChanged(nameof(Label));
        OnPropertyChanged(nameof(MaxNumberOfRoutes));
        OnPropertyChanged(nameof(Vibration));
        OnPropertyChanged(nameof(Notification));
    }

    public async Task Save()
    {
        await SqliteDataStore.SavePlanAsync(_plan);

        await Shell.Current.GoToAsync($"..");
    }

    public async Task Delete()
    {
        await SqliteDataStore.RemovePlanAsync(_plan.Id);

        await Shell.Current.GoToAsync($"..");
    }
}
