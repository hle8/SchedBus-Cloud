using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchedBus.Models;
using SchedBus.Services;
using System.Collections.ObjectModel;

namespace SchedBus.ViewModels;

public class PlansViewModel : ObservableRecipient
{
    private PlanDataService _dataService;
    private Plan _selectedActivity;

    public ObservableCollection<Plan> Plans => _dataService.Plans;

    public IRelayCommand GoToAddPlanCommand { get; }
    public IRelayCommand GoToEditPlanCommand { get; }

    public PlansViewModel()
    {
        _dataService = PlanDataService.Instance;

        GoToAddPlanCommand = new RelayCommand(GoToAddPlan);
        GoToEditPlanCommand = new RelayCommand<Plan>(GoToEditPlan);
    }

    private async void GoToAddPlan()
    {
        await Shell.Current.GoToAsync("planeditpage");
    }

    private void GoToEditPlan(Plan plan)
    {

    }
}
