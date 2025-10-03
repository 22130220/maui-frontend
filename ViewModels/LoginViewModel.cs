using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }
        public bool IsLandscape { get => _isLandscape; set => SetProperty(ref _isLandscape, value); }
        public bool IsDisableLandScape { get => _isDisableLandScape; set => SetProperty(ref _isDisableLandScape, value); }
        public ICommand LoginCommand { get; private set; }

        public LoginViewModel() 
        {
            LoginCommand = new AsyncRelayCommand(Login);
        }

        private async Task Login()
        {
            await Shell.Current.DisplayAlert("Ok", $"Login xem thế nào" + Email + Password, "Thử đi");
        }

    }
}
