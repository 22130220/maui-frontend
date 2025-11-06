using MauiFrontend.ViewModels;
using MauiFrontend.Services;

namespace MauiFrontend.Views
{
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage(UserService userService)
        {
            InitializeComponent();
            BindingContext = new ForgotPasswordViewModel(userService);
        }
    }
}
