using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using MauiFrontend.Views;

namespace MauiFrontend.ViewModels
{
    public partial class AccountPageViewModel : ObservableObject
    {
        [ObservableProperty]
        bool isLoggedIn;

        [ObservableProperty]
        bool isLoggedOut;

        [ObservableProperty]
        string userName;

        public AccountPageViewModel()
        {
            LoadUserState();
        }

        void LoadUserState()
        {
            var token = Preferences.Get("auth_token", null);
            var name = Preferences.Get("user_name", null);

            IsLoggedIn = !string.IsNullOrEmpty(token);
            IsLoggedOut = !IsLoggedIn;
            UserName = string.IsNullOrEmpty(name) ? "" : name;
        }

        [RelayCommand]
        async Task ToLoginPage()
        {
            await Shell.Current.GoToAsync($"///LoginPage");
        }

        [RelayCommand]
        async Task Logout()
        {
            Preferences.Remove("auth_token");
            Preferences.Remove("user_name");
            Preferences.Remove("user_email");
            Preferences.Remove("user_roles");

            if (Application.Current.MainPage is AppShell shell)
            {
                shell.UpdateAccountTitle();
            }

            LoadUserState();

            await App.Current.MainPage.DisplayAlert("Đăng xuất", "Bạn đã đăng xuất thành công", "OK");
        }
    }
}
