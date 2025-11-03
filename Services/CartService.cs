using MauiFrontend.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace MauiFrontend.Services
{
    public class CartService
    {
        private const string CART_KEY = "user_cart_items";
        private ObservableCollection<CartItem> _cartItems;

        public event EventHandler CartChanged;

        public CartService()
        {
            _cartItems = new ObservableCollection<CartItem>();
            LoadCartFromStorage();
        }

        public ObservableCollection<CartItem> GetCartItems()
        {
            return _cartItems;
        }

        public int GetTotalItems()
        {
            return _cartItems.Sum(x => x.Quantity);
        }

        public decimal GetTotalAmount()
        {
            return _cartItems.Sum(x => x.TotalPrice);
        }

        public void AddToCart(Product product, int quantity = 1)
        {
            if (product == null || quantity <= 0) return;

            var existingItem = _cartItems.FirstOrDefault(x => x.ProductID == product.ProductID);

            if (existingItem != null)
            {
                var newQuantity = existingItem.Quantity + quantity;
                if (newQuantity <= product.QuantityStock)
                {
                    existingItem.Quantity = newQuantity;
                }
                else
                {
                    existingItem.Quantity = product.QuantityStock;
                }
            }
            else
            {
                var cartItem = new CartItem
                {
                    ProductID = product.ProductID,
                    Name = product.Name,
                    Price = product.Price,
                    Thumbnail = product.Thumbnail,
                    Quantity = Math.Min(quantity, product.QuantityStock),
                    DiscountDefault = product.DiscountDefault,
                    QuantityStock = product.QuantityStock
                };
                _cartItems.Add(cartItem);
            }

            SaveCartToStorage();
            OnCartChanged();
        }

        public void AddToCart(ProductDetail product, int quantity = 1)
        {
            if (product == null || quantity <= 0) return;

            var existingItem = _cartItems.FirstOrDefault(x => x.ProductID == product.ProductID);

            if (existingItem != null)
            {
                var newQuantity = existingItem.Quantity + quantity;
                if (newQuantity <= product.QuantityStock)
                {
                    existingItem.Quantity = newQuantity;
                }
                else
                {
                    existingItem.Quantity = product.QuantityStock;
                }
            }
            else
            {
                var cartItem = new CartItem
                {
                    ProductID = product.ProductID,
                    Name = product.Name,
                    Price = product.Price,
                    Thumbnail = product.Thumbnail,
                    Quantity = Math.Min(quantity, product.QuantityStock),
                    DiscountDefault = product.DiscountDefault,
                    QuantityStock = product.QuantityStock
                };
                _cartItems.Add(cartItem);
            }

            SaveCartToStorage();
            OnCartChanged();
        }

        public void IncreaseQuantity(string productId)
        {
            var item = _cartItems.FirstOrDefault(x => x.ProductID == productId);
            if (item != null && item.Quantity < item.QuantityStock)
            {
                item.Quantity++;
                SaveCartToStorage();
                OnCartChanged();
            }
        }

        public void DecreaseQuantity(string productId)
        {
            var item = _cartItems.FirstOrDefault(x => x.ProductID == productId);
            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                    SaveCartToStorage();
                    OnCartChanged();
                }
                else
                {
                    RemoveFromCart(productId);
                }
            }
        }

        public void RemoveFromCart(string productId)
        {
            var item = _cartItems.FirstOrDefault(x => x.ProductID == productId);
            if (item != null)
            {
                _cartItems.Remove(item);
                SaveCartToStorage();
                OnCartChanged();
            }
        }

        public void ClearCart()
        {
            _cartItems.Clear();
            SaveCartToStorage();
            OnCartChanged();
        }

        public bool IsInCart(string productId)
        {
            return _cartItems.Any(x => x.ProductID == productId);
        }

        private void SaveCartToStorage()
        {
            try
            {
                var json = JsonSerializer.Serialize(_cartItems.ToList());
                Preferences.Set(CART_KEY, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CartService] Error saving cart: {ex.Message}");
            }
        }

        private void LoadCartFromStorage()
        {
            try
            {
                var json = Preferences.Get(CART_KEY, string.Empty);
                if (!string.IsNullOrEmpty(json))
                {
                    var items = JsonSerializer.Deserialize<List<CartItem>>(json);
                    if (items != null)
                    {
                        _cartItems.Clear();
                        foreach (var item in items)
                        {
                            _cartItems.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CartService] Error loading cart: {ex.Message}");
            }
        }

        private void OnCartChanged()
        {
            CartChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}