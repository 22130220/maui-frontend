using MauiFrontend.ViewModels;

namespace MauiFrontend.Views;

public partial class CartPage : ContentPage
{
    public CartPage(CartPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}