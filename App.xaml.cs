using MauiFrontend.ViewModels;
using MauiFrontend.Views;

namespace MauiFrontend
{
    public partial class App : Application
    {
        public static GlobalViewModel GlobalViewModel { get; } = new GlobalViewModel();
        public App()
        {
            InitializeComponent();
           
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());
            return window;

        }
    }
}