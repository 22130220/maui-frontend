using CommunityToolkit.Mvvm.ComponentModel;
using MauiFrontend.Models;
using MauiFrontend.Services;
using System.Collections.ObjectModel;
using MauiFrontend.Constants;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MauiFrontend.Views;

namespace MauiFrontend.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        private bool _isBusy = false;
        private ProductService _productService;
        private ObservableCollection<Product> _productList;
        private int page = 1;
        private int size = 20;

        public bool IsBusy { get => _isBusy; set => SetProperty(ref _isBusy, value); }
        public int Page { get => page; set => SetProperty(ref page, value); }
        public int Size { get => size; set => SetProperty(ref size, value); }

        public IAsyncRelayCommand<string> GoToDetailCommand { get; }
        public IAsyncRelayCommand LoadProductsCommand { get; }
        public IAsyncRelayCommand ToLoginPageCommand { get; }
        public ObservableCollection<Product> ProductList { get => _productList; set => SetProperty(ref _productList, value); }



        public HomePageViewModel(ProductService productService)
        {
            this._productService = productService;
            LoadProductsCommand = new AsyncRelayCommand(GetProductListAsync);
            ToLoginPageCommand = new AsyncRelayCommand(ToLoginPageAsync);
            _productList = new ObservableCollection<Product>();
            GoToDetailCommand = new AsyncRelayCommand<string>(GoToDetailAsync);

        }

        private async Task GetProductListAsync()
        {
            IsBusy = true;
            //App.GlobalViewModel.IsGlobalLoading = true;
            //App.GlobalViewModel.LoadingMessage = "Đang tải sản phẩm";

            var productResp = await _productService.GetSingleNoSeperatorAsync<ApiResponse<DataPaging<Product>>>(APICONSTANT.PRODUCT.GET_LIST, $"?page={Page}&size={Size}");
            ProductList.Clear();
            if (productResp != null)
            {
                await Task.Delay(100);

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    var productList = productResp.Data?.Content;
                    ProductList = new ObservableCollection<Product>(productList ?? []);

                });
            }

            IsBusy = false;
            //App.GlobalViewModel.IsGlobalLoading = false;
            //App.GlobalViewModel.LoadingMessage = "Đang tải sản phẩm";
        }

        private async Task ToLoginPageAsync()
        {
            await Shell.Current.GoToAsync($"///LoginPage");
        }

        private async Task GoToDetailAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return;
            }

            await Shell.Current.GoToAsync(nameof(ProductDetailPage), true, new Dictionary<string, object>
    {
        { "ProductID", productId }
    });
        }
    }
}