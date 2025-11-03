using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiFrontend.Models;
using MauiFrontend.Services;
using MauiFrontend.Views;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MauiFrontend.ViewModels
{
    public partial class CartPageViewModel : ObservableObject
    {
        private readonly CartService _cartService;

        public ObservableCollection<CartItem> CartItems { get; private set; }

        [ObservableProperty]
        private int totalItems;

        [ObservableProperty]
        private decimal totalAmount;

        [ObservableProperty]
        private string formattedTotalAmount;

        [ObservableProperty]
        private bool hasItems;

        public IAsyncRelayCommand<string> IncreaseQuantityCommand { get; }
        public IAsyncRelayCommand<string> DecreaseQuantityCommand { get; }
        public IAsyncRelayCommand<string> RemoveItemCommand { get; }
        public IAsyncRelayCommand ClearCartCommand { get; }
        public IAsyncRelayCommand CheckoutCommand { get; }
        public IAsyncRelayCommand<string> GoToProductDetailCommand { get; }

        public CartPageViewModel(CartService cartService)
        {
            _cartService = cartService;
            CartItems = _cartService.GetCartItems();

            // Lắng nghe sự thay đổi của collection
            CartItems.CollectionChanged += OnCartItemsCollectionChanged;

            // Lắng nghe sự thay đổi từ CartService
            _cartService.CartChanged += OnCartChanged;

            IncreaseQuantityCommand = new AsyncRelayCommand<string>(IncreaseQuantityAsync);
            DecreaseQuantityCommand = new AsyncRelayCommand<string>(DecreaseQuantityAsync);
            RemoveItemCommand = new AsyncRelayCommand<string>(RemoveItemAsync);
            ClearCartCommand = new AsyncRelayCommand(ClearCartAsync);
            CheckoutCommand = new AsyncRelayCommand(CheckoutAsync);
            GoToProductDetailCommand = new AsyncRelayCommand<string>(GoToProductDetailAsync);

            UpdateTotals();
        }

        private void OnCartItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Khi collection thay đổi (thêm/xóa item), cập nhật totals
            UpdateTotals();

            // Subscribe vào PropertyChanged của các item mới
            if (e.NewItems != null)
            {
                foreach (CartItem item in e.NewItems)
                {
                    item.PropertyChanged += OnCartItemPropertyChanged;
                }
            }

            // Unsubscribe khỏi các item bị xóa
            if (e.OldItems != null)
            {
                foreach (CartItem item in e.OldItems)
                {
                    item.PropertyChanged -= OnCartItemPropertyChanged;
                }
            }
        }

        private void OnCartItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Khi quantity của item thay đổi, cập nhật totals
            if (e.PropertyName == nameof(CartItem.Quantity) ||
                e.PropertyName == nameof(CartItem.TotalPrice))
            {
                UpdateTotals();
            }
        }

        private void OnCartChanged(object sender, EventArgs e)
        {
            UpdateTotals();
        }

        private void UpdateTotals()
        {
            TotalItems = _cartService.GetTotalItems();
            TotalAmount = _cartService.GetTotalAmount();
            FormattedTotalAmount = $"{TotalAmount:N0} ₫";
            HasItems = CartItems.Any();

            System.Diagnostics.Debug.WriteLine($"[CartPageViewModel] Updated - Items: {TotalItems}, Amount: {TotalAmount}");
        }

        private async Task IncreaseQuantityAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId)) return;

            _cartService.IncreaseQuantity(productId);
            UpdateTotals();
        }

        private async Task DecreaseQuantityAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId)) return;

            _cartService.DecreaseQuantity(productId);
            UpdateTotals();
        }

        private async Task RemoveItemAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId)) return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Xác nhận",
                "Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng?",
                "Xóa",
                "Hủy");

            if (confirm)
            {
                _cartService.RemoveFromCart(productId);
                UpdateTotals();
            }
        }

        private async Task ClearCartAsync()
        {
            if (!HasItems) return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Xác nhận",
                "Bạn có chắc muốn xóa toàn bộ giỏ hàng?",
                "Xóa tất cả",
                "Hủy");

            if (confirm)
            {
                _cartService.ClearCart();
                UpdateTotals();
            }
        }

        private async Task CheckoutAsync()
        {
            if (!HasItems)
            {
                await Shell.Current.DisplayAlert("Thông báo", "Giỏ hàng trống!", "OK");
                return;
            }

            await Shell.Current.DisplayAlert(
                "Thông báo",
                $"Chức năng thanh toán đang phát triển.\nTổng tiền: {FormattedTotalAmount}",
                "OK");
        }

        private async Task GoToProductDetailAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId)) return;

            await Shell.Current.GoToAsync(nameof(ProductDetailPage), true, new Dictionary<string, object>
            {
                { "ProductID", productId }
            });
        }
    }
}