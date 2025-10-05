using MauiFrontend.ViewModels;
using MauiIcons.Core;

namespace MauiFrontend.Views;

public partial class HomePage : ContentPage
{
	public HomePage(HomePageViewModel homePageViewModel)
	{
		InitializeComponent();
        _ = new MauiIcon();
        BindingContext = homePageViewModel;
    }


    /*
    * ================================
    *  Cách kiểm tra điều kiện để chuyển trang khi trang đó xuất hiện
    * ================================
    */

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //// viết logic xử lý điều kiện ở đây
        //await Shell.Current.GoToAsync(nameof(LoginPage));

        if (BindingContext is HomePageViewModel vm)
        {
            await vm.LoadProductsCommand.ExecuteAsync(null);
        }
    }
}