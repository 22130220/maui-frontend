using MauiFrontend.Views;

namespace MauiFrontend
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
           
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());

            // Khi app khởi động, điều hướng tùy logic
            Task.Run(async () =>
            {
                await Shell.Current.GoToAsync(nameof(LoginPage));
            });

            return window;

        }
    }
}