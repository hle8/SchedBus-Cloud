using SchedBus.Services;

namespace SchedBus
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<PlanDataService>();

            MainPage = new AppShell();
        }
    }
}
