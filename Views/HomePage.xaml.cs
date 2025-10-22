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

    private async void OnSearchCompleted(object sender, EventArgs e)
    {
        if (BindingContext is not HomePageViewModel vm)
            return;

        vm.IsSuggestionsVisible = false;
        var keyword = vm.SearchText?.Trim();
        if (string.IsNullOrWhiteSpace(keyword))
            return;

        Console.WriteLine($"[Enter] Navigating to SearchResultsPage with keyword: {keyword}");
        await Shell.Current.GoToAsync(nameof(SearchResultsPage), true, new Dictionary<string, object>
    {
        { "Keyword", keyword }
    });
    }
}