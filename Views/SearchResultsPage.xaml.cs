using MauiFrontend.ViewModels;

namespace MauiFrontend.Views;

public partial class SearchResultsPage : ContentPage
{
    public SearchResultsPage(SearchResultsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SearchResultsViewModel vm)
        {
            await vm.LoadResultsAsync();
        }
    }
}