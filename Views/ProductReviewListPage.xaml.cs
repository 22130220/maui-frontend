namespace MauiFrontend.ViewModels;


public partial class ProductReviewListPage : ContentPage
{

	public ProductReviewListPage(ProductReviewListViewModel productReviewListViewModel)
	{
		InitializeComponent();
		BindingContext = productReviewListViewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var viewModel = (ProductReviewListViewModel)BindingContext;
        if (!string.IsNullOrEmpty(viewModel.ProductID))
        {
            await viewModel.LoadReviewsAsync();
        }
    }
}