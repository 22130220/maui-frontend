using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System;
using MauiFrontend.Services;


namespace MauiFrontend.ViewModels
{
    [QueryProperty(nameof(OrderID), nameof(OrderID))]
    [QueryProperty(nameof(TotalAmount), nameof(TotalAmount))]
    [QueryProperty(nameof(OrderDate), nameof(OrderDate))]
    public partial class OrderSuccessPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private int orderID;

        [ObservableProperty]
        private long totalAmount;

        [ObservableProperty]
        private string orderDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

        [ObservableProperty]
        private string formattedTotalAmount = "0 ₫";

        public IAsyncRelayCommand GoToHomeCommand { get; }
        public IAsyncRelayCommand ViewOrderDetailCommand { get; }

        public OrderSuccessPageViewModel()
        {
            GoToHomeCommand = new AsyncRelayCommand(GoToHomeAsync);
            ViewOrderDetailCommand = new AsyncRelayCommand(ViewOrderDetailAsync);
        }

        partial void OnTotalAmountChanged(long value)
        {
            FormattedTotalAmount = $"{value:N0} ₫";
        }

        private async Task GoToHomeAsync()
        {
            // Quay về trang chủ và clear navigation stack
            await Shell.Current.GoToAsync("///HomePage");
        }

        private async Task ViewOrderDetailAsync()
        {
            // TODO: Navigate to order detail page
            await Shell.Current.DisplayAlert("Thông báo", $"Chi tiết đơn hàng #{OrderID}", "OK");
        }
    }
}