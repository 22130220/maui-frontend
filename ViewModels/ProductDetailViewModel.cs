using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiFrontend.Models;
using MauiFrontend.Services;
using System.Threading.Tasks;

namespace MauiFrontend.ViewModels
{
    [QueryProperty("ProductID", "ProductID")]
    public partial class ProductDetailViewModel : ObservableObject
    {
        private readonly ProductService _productService;
        private readonly CartService _cartService;

        [ObservableProperty]
        private string productID;

        [ObservableProperty]
        private ProductDetail product;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private int quantity = 1;

        [ObservableProperty]
        private bool canAddToCart = true;

        [ObservableProperty]
        private bool canIncrease = true;

        [ObservableProperty]
        private bool canDecrease = false;

        public IAsyncRelayCommand AddToCartCommand { get; }
        public IRelayCommand IncreaseQuantityCommand { get; }
        public IRelayCommand DecreaseQuantityCommand { get; }

        public ProductDetailViewModel(ProductService productService, CartService cartService)
        {
            _productService = productService;
            _cartService = cartService;

            AddToCartCommand = new AsyncRelayCommand(AddToCartAsync, () => CanAddToCart);
            IncreaseQuantityCommand = new RelayCommand(IncreaseQuantity, () => CanIncrease);
            DecreaseQuantityCommand = new RelayCommand(DecreaseQuantity, () => CanDecrease);
        }

        partial void OnProductIDChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
                _ = LoadProductDetailAsync(value);
        }

        partial void OnQuantityChanged(int value)
        {
            UpdateButtonStates();
            System.Diagnostics.Debug.WriteLine($"[ProductDetailViewModel] Quantity changed to: {value}");
        }

        partial void OnProductChanged(ProductDetail value)
        {
            UpdateButtonStates();
        }

        private async Task LoadProductDetailAsync(string id)
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var result = await _productService.GetByIdAsync(id);
                if (result != null)
                {
                    Product = result;
                    Quantity = 1;
                    UpdateButtonStates();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void IncreaseQuantity()
        {
            if (Product != null && Quantity < Product.QuantityStock)
            {
                Quantity++;
                System.Diagnostics.Debug.WriteLine($"[ProductDetailViewModel] Increased to: {Quantity}");
            }
        }

        private void DecreaseQuantity()
        {
            if (Quantity > 1)
            {
                Quantity--;
                System.Diagnostics.Debug.WriteLine($"[ProductDetailViewModel] Decreased to: {Quantity}");
            }
        }

        private async Task AddToCartAsync()
        {
            if (Product == null || !CanAddToCart) return;

            _cartService.AddToCart(Product, Quantity);

            await Shell.Current.DisplayAlert(
                "Thành công",
                $"Đã thêm {Quantity} sản phẩm vào giỏ hàng!",
                "OK");

            Quantity = 1;
        }

        private void UpdateButtonStates()
        {
            CanAddToCart = Product != null &&
                          Quantity > 0 &&
                          Quantity <= Product.QuantityStock;

            canIncrease = Product != null && Quantity < Product.QuantityStock;
            canDecrease = Quantity > 1;

            AddToCartCommand.NotifyCanExecuteChanged();
            IncreaseQuantityCommand.NotifyCanExecuteChanged();
            DecreaseQuantityCommand.NotifyCanExecuteChanged();

            System.Diagnostics.Debug.WriteLine($"[ProductDetailViewModel] States - CanAdd: {CanAddToCart}, CanInc: {CanIncrease}, CanDec: {CanDecrease}");
        }
    }
}