using MauiFrontend.ViewModels;

namespace MauiFrontend.Views;

public partial class AccountPage : ContentPage
{
	public AccountPage(AccountPageViewModel accountPageViewModel)
	{
		InitializeComponent();
		BindingContext = accountPageViewModel;
	}
}