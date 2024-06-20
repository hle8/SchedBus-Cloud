using SchedBus.ViewModels;

namespace SchedBus.Pages;

public partial class TimeSetPage : ContentPage
{
    public TimeSetPage(TimeSetViewModel timeSetViewModel)
    {
        InitializeComponent();
        BindingContext = timeSetViewModel;
    }
}