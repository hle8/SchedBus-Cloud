using SchedBus.Pages;

namespace SchedBus
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(PlanPage), typeof(PlanPage));
            Routing.RegisterRoute(nameof(PlanEditPage), typeof(PlanEditPage));
            Routing.RegisterRoute(nameof(TimeSetPage), typeof(TimeSetPage));
        }
    }
}
