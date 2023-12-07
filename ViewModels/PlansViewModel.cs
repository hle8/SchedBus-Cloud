using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using SchedBus.Services;
using System.Windows.Input;

namespace SchedBus.ViewModels;

internal class PlansViewModel
{
    public IDataStore SqliteDataStore => DependencyService.Get<IDataStore>();
    public ObservableRangeCollection<PlanEditViewModel> Plans { get; set; }
    public ICommand PageAppearingCommand { get; set; }
    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }

    public PlansViewModel()
    {
        Plans = new ObservableRangeCollection<PlanEditViewModel>();
        PageAppearingCommand = new AsyncRelayCommand(PageAppearing);
        AddCommand = new AsyncRelayCommand(AddAsync);
        EditCommand = new AsyncRelayCommand<PlanEditViewModel>(EditAsync);
        DeleteCommand = new AsyncRelayCommand<PlanEditViewModel>(DeleteAsync);
    }

    public async Task Refresh()
    {
        var plans = await SqliteDataStore.GetPlansAsync();
        Plans.Clear();
        Plans.AddRange(new List<PlanEditViewModel>(plans.Select(plan => new PlanEditViewModel(plan))));
    }

    async Task PageAppearing() => await Refresh();

    async Task AddAsync() => await Shell.Current.GoToAsync(nameof(Pages.PlanEditPage));

    async Task EditAsync(PlanEditViewModel plan)
    {
        await Shell.Current.GoToAsync($"{nameof(Pages.PlanEditPage)}?selectedplan={plan.Id}");
    }

    async Task DeleteAsync(PlanEditViewModel plan)
    {
        await SqliteDataStore.RemovePlanAsync(plan.Id);
    }
}