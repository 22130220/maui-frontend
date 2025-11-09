using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiFrontend.Constants;
using MauiFrontend.Models;
using MauiFrontend.Services;
using MauiFrontend.Views;
using System.Windows.Input;

namespace MauiFrontend.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private string _email;
        private string _password;
        private bool _isLoading;
        private bool _isLandscape;
        private bool _isDisableLandScape;

        private UserService _userService;
        private ProductService _productService;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsLandscape
        {
            get => _isLandscape;
            set => SetProperty(ref _isLandscape, value);
        }

        public bool IsDisableLandScape
        {
            get => _isDisableLandScape;
            set => SetProperty(ref _isDisableLandScape, value);
        }

        public ICommand LoginCommand { get; private set; }
        public ICommand ForgotPasswordCommand { get; private set; }

        public LoginViewModel(UserService userService, ProductService productService)
        {
            _userService = userService;
            _productService = productService;
            LoginCommand = new AsyncRelayCommand(Login);
            ForgotPasswordCommand = new AsyncRelayCommand(ForgotPassword);
        }

        private async Task Login()
        {
            // Validation
            if (string.IsNullOrWhiteSpace(Email))
            {
                await Shell.Current.DisplayAlert("Lỗi", "Vui lòng nhập email hoặc tên đăng nhập", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Lỗi", "Vui lòng nhập mật khẩu", "OK");
                return;
            }

            try
            {
                System.Diagnostics.Debug.WriteLine("trường hợp try");
                IsLoading = true;
                // Gọi API đăng nhập
                var response = await _userService.LoginAsync(Email.Trim(), Password);

                System.Diagnostics.Debug.WriteLine(" response api: ", response);
                if (response != null && response.Code == 200 && response.Data != null)
                {

                    // Đăng nhập thành công
                    //System.Diagnostics.Debug.WriteLine($"Đăng nhập thành công: {response.Data.Email}");
                    // Hiển thị thông báo chào mừng
                    await Shell.Current.DisplayAlert(
                        "Thành công",
                        $"Chào mừng {response.Data.Email}!",
                        "OK"
                    );

                    // Chuyển đến trang chủ
                    await Shell.Current.GoToAsync($"///HomePage");
                }
                else
                {
                    // Đăng nhập thất bại
                    var errorMessage = response?.Message ?? "Không tìm thấy tài khoản. Vui lòng kiểm tra lại email và mật khẩu.";

                    await Shell.Current.DisplayAlert(
                        "Đăng nhập thất bại",
                        errorMessage,
                        "OK"
                    );

                    System.Diagnostics.Debug.WriteLine($"Đăng nhập thất bại: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("trường hợp catch");
                System.Diagnostics.Debug.WriteLine($"Lỗi khi đăng nhập: {ex.Message}");

                await Shell.Current.DisplayAlert(
                    "Lỗi",
                    "Có lỗi xảy ra khi đăng nhập. Vui lòng thử lại sau.",
                    "OK"
                );
            }
            finally
            {
                IsLoading = false;
            }
        }
        private async Task ForgotPassword()
        {
            await Shell.Current.GoToAsync(nameof(ForgotPasswordPage));
        }
    }
}