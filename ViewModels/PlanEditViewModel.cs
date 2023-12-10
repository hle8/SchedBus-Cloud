using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchedBus.Models;
using SchedBus.Services;
using System.Collections.ObjectModel;

namespace SchedBus.ViewModels;

public class PlanEditViewModel : ObservableObject, IQueryAttributable
{
    protected static PlanDataService Database => PlanDataService.Instance;

    public Plan Plan { get; set; }
    public ObservableCollection<TimeSet> TimeSet { get; set; }

    public IAsyncRelayCommand GetCommand { get; }
    public IAsyncRelayCommand SelectCommand { get; }
    public IAsyncRelayCommand SaveCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }

    public PlanEditViewModel()
    {
        GetCommand = new AsyncRelayCommand(GetAsync);
        SelectCommand = new AsyncRelayCommand<TimeSet>(SelectAsync);
        SaveCommand = new AsyncRelayCommand(SaveAsync);
        DeleteCommand = new AsyncRelayCommand(DeleteAsync);
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("selectedplan"))
        {
            Plan = query["selectedplan"] as Plan;

            OnPropertyChanged(nameof(Plan));
        }
    }

    public async Task GetAsync()
    {
        TimeSet = await Database.GetTimeSetOfPlanAsync(Plan.Id);
        OnPropertyChanged(nameof(TimeSet));
    }

    public async Task SelectAsync(TimeSet timSet)
    {
        var navigationParameter = new Dictionary<string, object>() { { "selectedtimeset", timSet } };
        await Shell.Current.GoToAsync($"{nameof(Pages.TimeSetPage)}", navigationParameter);
    }

    public async Task SaveAsync()
    {
        // await Database.SavePlanAsync(_plan);
        await Shell.Current.GoToAsync($"..");
    }

    public async Task DeleteAsync()
    {
        // await Database.DeleteDbAsync(_plan);
        await Shell.Current.GoToAsync($"..");
    }
}
