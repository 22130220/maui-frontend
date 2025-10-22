using CommunityToolkit.Mvvm.ComponentModel;
using MauiFrontend.Models;
using MauiFrontend.Services;
using System.Threading.Tasks;

namespace MauiFrontend.ViewModels
{
    [QueryProperty("ProductID", "ProductID")]
    public partial class ProductDetailViewModel : ObservableObject
    {
        private readonly ProductService _productService;

        [ObservableProperty]
        private string productID;

        [ObservableProperty]
        private ProductDetail product; 

        [ObservableProperty]
        private bool isBusy;

        public ProductDetailViewModel(ProductService productService)
        {
            _productService = productService;
        }

        partial void OnProductIDChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
                _ = LoadProductDetailAsync(value);
        }

        private async Task LoadProductDetailAsync(string id)
        {
            if (IsBusy) return;
            IsBusy = true;

            var result = await _productService.GetByIdAsync(id);
            if (result != null)
            {
                Product = result;
            }

            IsBusy = false;
        }
    }
}
