using SchedBus.ViewModels;

namespace SchedBus.Pages;

public partial class PlanEditPage : ContentPage
{
    public PlanEditPage(PlanEditViewModel planEditViewModel)
    {
        InitializeComponent();
        BindingContext = planEditViewModel;
    }
}