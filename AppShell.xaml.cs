//using MauiFrontend.ViewModels;
//using MauiFrontend.Views;

//namespace MauiFrontend
//{
//    public partial class AppShell : Shell
//    {
//        public AppShell()
//        {
//            InitializeComponent();
//            Routing.RegisterRoute(nameof(ProductDetailPage), typeof(ProductDetailPage));
//            Routing.RegisterRoute(nameof(SearchResultsPage), typeof(SearchResultsPage));
//            Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
//            Routing.RegisterRoute(nameof(ProductReviewListPage), typeof(ProductReviewListPage));
//        }
//    }
//}

using MauiFrontend.ViewModels;
using MauiFrontend.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace MauiFrontend
{
    public partial class AppShell : Shell
    {
        // Tạo BindableProperty để binding Title
        public static readonly BindableProperty AccountTitleProperty =
            BindableProperty.Create(
                nameof(AccountTitle),
                typeof(string),
                typeof(AppShell),
                "Tài khoản",
                BindingMode.TwoWay);

        public string AccountTitle
        {
            get => (string)GetValue(AccountTitleProperty);
            set => SetValue(AccountTitleProperty, value);
        }

        public AppShell()
        {
            InitializeComponent();
            BindingContext = this;

            // Đăng ký route
            Routing.RegisterRoute(nameof(ProductDetailPage), typeof(ProductDetailPage));
            Routing.RegisterRoute(nameof(SearchResultsPage), typeof(SearchResultsPage));
            Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
            Routing.RegisterRoute(nameof(ProductReviewListPage), typeof(ProductReviewListPage));

            // Khởi tạo Title
            UpdateAccountTitle();
        }

        public void UpdateAccountTitle()
        {
            var userName = Preferences.Get("user_name", null);
            var newTitle = string.IsNullOrEmpty(userName) ? "Tài khoản" : userName;

            System.Diagnostics.Debug.WriteLine($"Updating AccountTitle from '{AccountTitle}' to '{newTitle}'");
            System.Diagnostics.Debug.WriteLine($" user_name in Preferences: {userName}");

            // Cập nhật property
            AccountTitle = newTitle;

            // Force UI update
            OnPropertyChanged(nameof(AccountTitle));

            System.Diagnostics.Debug.WriteLine($"AccountTitle updated: {AccountTitle}");
        }
    }
}