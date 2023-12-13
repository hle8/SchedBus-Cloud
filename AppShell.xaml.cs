using SchedBus.Pages;

namespace SchedBus
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(PlanEditPage), typeof(PlanEditPage));
            Routing.RegisterRoute(nameof(TimeSetPage), typeof(TimeSetPage));
            Routing.RegisterRoute(nameof(LocationSearch), typeof(LocationSearch));
        }
    }
}
