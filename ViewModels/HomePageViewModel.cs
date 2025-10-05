using CommunityToolkit.Mvvm.ComponentModel;
using MauiFrontend.Models;
using MauiFrontend.Services;
using System.Collections.ObjectModel;
using MauiFrontend.Constants;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace MauiFrontend.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        private bool _isBusy = false;
        private ProductService _productService;
        private ObservableCollection<Product> _productList;

        public bool IsBusy { get => _isBusy; set => SetProperty(ref _isBusy, value); }
        public IAsyncRelayCommand LoadProductsCommand { get; }
        public IAsyncRelayCommand ToLoginPageCommand { get; }
        public ObservableCollection<Product> ProductList { get => _productList; set => SetProperty(ref _productList, value); }


        public HomePageViewModel(ProductService productService)
        {
            this._productService = productService;
            LoadProductsCommand = new AsyncRelayCommand(GetProductListAsync);
            ToLoginPageCommand = new AsyncRelayCommand(ToLoginPageAsync);
            _productList = new ObservableCollection<Product>();

        }
        
        private async Task GetProductListAsync()
        {
            IsBusy = true;
            //App.GlobalViewModel.IsGlobalLoading = true;
            //App.GlobalViewModel.LoadingMessage = "Đang tải sản phẩm";

            var productList = await _productService.GetListAsync(APICONSTANT.PRODUCT.GET_LIST);
            ProductList.Clear();
            if (productList != null)
            {
                await Task.Delay(100);

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    ProductList = new ObservableCollection<Product>(productList);

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


    }
}
