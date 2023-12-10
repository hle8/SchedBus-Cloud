using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using SchedBus.Models;
using SchedBus.Services;
using System.Collections.ObjectModel;

namespace SchedBus.ViewModels;

public class PlansViewModel : ObservableObject
{
    protected static PlanDataService Database => PlanDataService.Instance;
    public ObservableCollection<Plan> Plans { get; set; }
    public IAsyncRelayCommand GetCommand { get; }
    public IAsyncRelayCommand AddCommand { get; }
    public IAsyncRelayCommand EditCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }

    public PlansViewModel()
    {
        GetCommand = new AsyncRelayCommand(GetAsync);
        AddCommand = new AsyncRelayCommand(AddAsync);
        EditCommand = new AsyncRelayCommand<Plan>(EditAsync);
        DeleteCommand = new AsyncRelayCommand<Plan>(DeleteAsync);
    }

    public async Task GetAsync()
    {
        Plans = await Database.GetDbAsync<Plan>();
        OnPropertyChanged(nameof(Plans));
    }

    async Task AddAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(Pages.PlanEditPage)}");
    }

    async Task EditAsync(Plan plan)
    {
        var navigationParameter = new Dictionary<string, object>() { { "selectedplan", plan } };
        await Shell.Current.GoToAsync($"{nameof(Pages.PlanEditPage)}", navigationParameter);
    }

    async Task DeleteAsync(Plan plan)
    {
        await Database.DeleteDbAsync(plan);
    }
}