using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiFrontend.Models;
using MauiFrontend.Models.Request;
using MauiFrontend.Models.Response;
using MauiFrontend.Services;
using MauiFrontend.Views;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace MauiFrontend.ViewModels
{
    [QueryProperty(nameof(TotalAmount), nameof(TotalAmount))]
    public partial class CheckoutPageViewModel : ObservableObject
    {
        private readonly CheckoutService _checkoutService;
        private readonly CartService _cartService;

        [ObservableProperty]
        private string fullname = "";

        [ObservableProperty]
        private string email = "";

        [ObservableProperty]
        private string phoneNumber = "";

        [ObservableProperty]
        private string address = "";

        [ObservableProperty]
        private string note = "";

        [ObservableProperty]
        private int fee = 30000;  // GÁN CỨNG 30,000đ

        [ObservableProperty]
        private int discount = 0;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FormattedTotalAmount))]
        [NotifyPropertyChangedFor(nameof(FormattedFinalTotal))]
        private decimal totalAmount = 0;

        [ObservableProperty]
        private bool isProcessing = false;

        [ObservableProperty]
        private string formattedTotalAmount = "0 ₫";

        [ObservableProperty]
        private string formattedFinalTotal = "0 ₫";

        [ObservableProperty]
        private bool isFormValid = false;

        // ĐỔI THÀNH OBSERVABLEPROPERTY ĐỂ TỰ ĐỘNG TẠO PROPERTY PASCALCASE
        [ObservableProperty]
        private ObservableCollection<CartItem> cartItems;

        public IAsyncRelayCommand SubmitCheckoutCommand { get; }
        public IAsyncRelayCommand GoBackCommand { get; }

        public CheckoutPageViewModel(CheckoutService checkoutService, CartService cartService)
        {
            _checkoutService = checkoutService;
            _cartService = cartService;

            // Lấy cart items từ service
            cartItems = _cartService.GetCartItems();

            // Tính tổng tiền từ cart service
            totalAmount = _cartService.GetTotalAmount();

            System.Diagnostics.Debug.WriteLine($"[CheckoutPageViewModel] Cart items count: {cartItems?.Count ?? 0}");
            System.Diagnostics.Debug.WriteLine($"[CheckoutPageViewModel] Total amount from cart: {totalAmount}");

            SubmitCheckoutCommand = new AsyncRelayCommand(SubmitCheckoutAsync, () => isFormValid && !isProcessing);
            GoBackCommand = new AsyncRelayCommand(GoBackAsync);

            // Subscribe để validate form khi thay đổi
            PropertyChanged += OnPropertyChanged;

            // Tính toán ban đầu
            UpdateTotals();
            ValidateForm();
        }

        partial void OnTotalAmountChanged(decimal value)
        {
            UpdateTotals();
        }

        partial void OnFeeChanged(int value)
        {
            UpdateTotals();
        }

        partial void OnDiscountChanged(int value)
        {
            UpdateTotals();
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Validate form khi bất kỳ field nào thay đổi
            if (e.PropertyName is nameof(Fullname) or nameof(Email) or nameof(PhoneNumber) or nameof(Address))
            {
                ValidateForm();
            }
        }

        private void UpdateTotals()
        {
            decimal finalTotal = totalAmount + fee - discount;
            formattedTotalAmount = $"{totalAmount:N0} ₫";
            formattedFinalTotal = $"{finalTotal:N0} ₫";

            System.Diagnostics.Debug.WriteLine($"[CheckoutPageViewModel] UpdateTotals - Total: {totalAmount}, Fee: {fee}, Discount: {discount}, Final: {finalTotal}");
        }

        private void ValidateForm()
        {
            bool isValid = !string.IsNullOrWhiteSpace(fullname) &&
                           !string.IsNullOrWhiteSpace(email) &&
                           !string.IsNullOrWhiteSpace(phoneNumber) &&
                           !string.IsNullOrWhiteSpace(address) &&
                           IsValidEmail(email) &&
                           IsValidPhoneNumber(phoneNumber);

            isFormValid = isValid;
            SubmitCheckoutCommand.NotifyCanExecuteChanged();

            System.Diagnostics.Debug.WriteLine($"[CheckoutPageViewModel] Form validation: {isValid}");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            string cleanPhone = Regex.Replace(phone, @"\D", "");
            return cleanPhone.Length >= 10 && cleanPhone.Length <= 11;
        }

        private async Task SubmitCheckoutAsync()
        {
            if (isProcessing || !isFormValid)
            {
                System.Diagnostics.Debug.WriteLine($"[CheckoutPageViewModel] Cannot submit - Processing: {isProcessing}, Valid: {isFormValid}");
                return;
            }

            try
            {
                isProcessing = true;
                SubmitCheckoutCommand.NotifyCanExecuteChanged();

                // Validate giỏ hàng không trống
                if (cartItems == null || cartItems.Count == 0)
                {
                    await Shell.Current.DisplayAlert("Thông báo", "Giỏ hàng trống", "OK");
                    return;
                }

                // Tính tổng tiền cuối cùng
                long finalTotal = (long)(totalAmount + fee - discount);

                // Tạo request
                var checkoutRequest = new CheckoutRequest
                {
                    fullname = fullname.Trim(),
                    email = email.Trim(),
                    phoneNumber = Regex.Replace(phoneNumber, @"\D", ""),
                    address = address.Trim(),
                    fee = fee,
                    discount = discount,
                    note = note.Trim(),
                    items = cartItems.Select(c => new OrderItemRequest
                    {
                        productID = c.ProductID,
                        quantity = c.Quantity,
                        totalPrice = (long)c.TotalPrice
                    }).ToList()
                };

                System.Diagnostics.Debug.WriteLine($"[CheckoutPageViewModel] ===== REQUEST INFO =====");
                System.Diagnostics.Debug.WriteLine($"Fullname: {checkoutRequest.fullname}");
                System.Diagnostics.Debug.WriteLine($"Email: {checkoutRequest.email}");
                System.Diagnostics.Debug.WriteLine($"Phone: {checkoutRequest.phoneNumber}");
                System.Diagnostics.Debug.WriteLine($"Address: {checkoutRequest.address}");
                System.Diagnostics.Debug.WriteLine($"Items count: {checkoutRequest.items.Count}");
                System.Diagnostics.Debug.WriteLine($"Fee: {checkoutRequest.fee}");
                System.Diagnostics.Debug.WriteLine($"Discount: {checkoutRequest.discount}");
                System.Diagnostics.Debug.WriteLine($"Final Total: {finalTotal}");

                // Gọi API
                var response = await _checkoutService.CheckoutAsync(checkoutRequest);

                System.Diagnostics.Debug.WriteLine($"[CheckoutPageViewModel] ===== RESPONSE INFO =====");
                System.Diagnostics.Debug.WriteLine($"Response is null: {response == null}");
                if (response != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Response Code: {response.Code}");
                    System.Diagnostics.Debug.WriteLine($"Response Message: {response.Message}");
                    System.Diagnostics.Debug.WriteLine($"Response Data is null: {response.Data == null}");
                    if (response.Data != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Order ID: {response.Data.orderID}");
                        System.Diagnostics.Debug.WriteLine($"Total Amount: {response.Data.totalAmount}");
                    }
                }

                if (response?.Data != null && response.Code == 201)
                {
                    System.Diagnostics.Debug.WriteLine($"[CheckoutPageViewModel] ✅ Checkout successful");

                    // Clear giỏ hàng
                    _cartService.ClearCart();

                    // Navigate đến trang thành công
                    await Shell.Current.GoToAsync(nameof(OrderSuccessPage), true, new Dictionary<string, object>
            {
                { "OrderID", response.Data.orderID },
                { "TotalAmount", response.Data.totalAmount },
                { "OrderDate", DateTime.Now.ToString("dd/MM/yyyy HH:mm") }
            });
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[CheckoutPageViewModel] ❌ Checkout failed");
                    await Shell.Current.DisplayAlert(
                        "Lỗi",
                        response?.Message ?? "Thanh toán thất bại",
                        "OK");
                }
            }
            catch (ArgumentException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CheckoutPageViewModel] ❌ Validation error: {ex.Message}");
                await Shell.Current.DisplayAlert("Lỗi", $"Dữ liệu không hợp lệ: {ex.Message}", "OK");
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CheckoutPageViewModel] ❌ HTTP error: {ex.Message}");
                await Shell.Current.DisplayAlert("Lỗi", "Không thể kết nối đến server. Vui lòng kiểm tra kết nối mạng", "OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CheckoutPageViewModel] ❌ Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[CheckoutPageViewModel] StackTrace: {ex.StackTrace}");
                await Shell.Current.DisplayAlert("Lỗi", $"Thanh toán thất bại: {ex.Message}", "OK");
            }
            finally
            {
                isProcessing = false;
                SubmitCheckoutCommand.NotifyCanExecuteChanged();
            }
        }
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("///CartPage");
        }
    }
}