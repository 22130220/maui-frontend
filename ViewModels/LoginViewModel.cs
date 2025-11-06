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

        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }
        public bool IsLandscape { get => _isLandscape; set => SetProperty(ref _isLandscape, value); }
        public bool IsDisableLandScape { get => _isDisableLandScape; set => SetProperty(ref _isDisableLandScape, value); }

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
            bool ok = await Shell.Current.DisplayAlert("Ok", $"Login xem thế nào" + Email + Password, "Vào vào", "Không vào");
            if (ok)
            {
                await Shell.Current.GoToAsync($"///HomePage");
            }
        }

        private async Task ForgotPassword()
        {
            await Shell.Current.GoToAsync(nameof(ForgotPasswordPage));
        }
    }
}
