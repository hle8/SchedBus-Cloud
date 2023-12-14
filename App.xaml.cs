using SchedBus.Pages;
using SchedBus.Services;

namespace SchedBus
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var newWindow = new NavigationPage(new SplashPage());
            MainPage = newWindow;
            // MainPage = new AppShell();
        }
    }
}
