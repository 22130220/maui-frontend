using MauiFrontend.ViewModels;

namespace MauiFrontend.Views
{
    public partial class OrderSuccessPage : ContentPage
    {

        public OrderSuccessPage() // bắt buộc có
        {
            InitializeComponent();
            BindingContext = new OrderSuccessPageViewModel(); // tự tạo hoặc lấy từ DI container
        }

        public OrderSuccessPage(OrderSuccessPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}