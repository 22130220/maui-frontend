using MauiFrontend.ViewModels;
using MauiFrontend.Views;

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
            Routing.RegisterRoute(nameof(CheckoutPage), typeof(CheckoutPage));
            Routing.RegisterRoute(nameof(OrderSuccessPage), typeof(OrderSuccessPage));  // THÊM DÒNG NÀY
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
        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            // Nếu đang chuyển qua 1 tab khác
            if (args.Source == ShellNavigationSource.ShellSectionChanged)
            {
                // Lấy đường dẫn hiện tại (trước khi chuyển tab)
                var current = Shell.Current.CurrentState.Location.OriginalString;
                var target = args.Target.Location.OriginalString;

                // 🔍 Nếu đang ở SuccessPage và chuyển qua tab khác -> reset về CartPage
                if (current.Contains("successpage", StringComparison.OrdinalIgnoreCase))
                {
                    await Shell.Current.GoToAsync("//CartPage");
                    return;
                }

                // Ngược lại, vẫn giữ behavior cũ (reset khi đổi tab)
                //if (!string.IsNullOrEmpty(target))
                //{
                //    await Shell.Current.GoToAsync("//" + target.Split('/')[1]);
                //}
            }
        }

    }
}