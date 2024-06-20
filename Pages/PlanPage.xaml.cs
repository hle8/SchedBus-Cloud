using SchedBus.ViewModels;

namespace SchedBus.Pages;

public partial class PlanPage : ContentPage
{
    public PlanPage(PlansViewModel plansViewModel)
    {
        InitializeComponent();
        BindingContext = plansViewModel;
    }
}