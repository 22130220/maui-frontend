using MauiFrontend.ViewModels;
using MauiFrontend.Views;

namespace MauiFrontend
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ProductDetailPage), typeof(ProductDetailPage));
            Routing.RegisterRoute(nameof(SearchResultsPage), typeof(SearchResultsPage));
            Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
            Routing.RegisterRoute(nameof(ProductReviewListPage), typeof(ProductReviewListPage));
            Routing.RegisterRoute(nameof(CheckoutPage), typeof(CheckoutPage));
            Routing.RegisterRoute(nameof(OrderSuccessPage), typeof(OrderSuccessPage));  // THÊM DÒNG NÀY

        }
    }
}
