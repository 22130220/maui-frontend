using MauiFrontend.ViewModels;

namespace MauiFrontend.Views;

public partial class CheckoutPage : ContentPage
{
    public CheckoutPage(CheckoutPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
