using MauiFrontend.ViewModels;

namespace MauiFrontend.Views;

public partial class LoginPage : ContentPage
{
    private string _lastState = "";
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginViewModel();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (BindingContext is LoginViewModel vm)
        {
            string newState = width > height ? "Landscape" : "Portrait";

            if (_lastState != newState)
            {
                _lastState = newState;
                vm.IsLandscape = newState != "Portrait";
                vm.IsDisableLandScape = !vm.IsLandscape;

                VisualStateManager.GoToState(MainGrid, newState);
            }
        }
    }
}