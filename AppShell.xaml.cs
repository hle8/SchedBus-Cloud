using SchedBus.Pages;

namespace SchedBus
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("planeditpage", typeof(PlanEditPage));
        }
    }
}
