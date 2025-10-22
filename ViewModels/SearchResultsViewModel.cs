using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiFrontend.Models;
using MauiFrontend.Services;
using MauiFrontend.Views;
using System.Collections.ObjectModel;

namespace MauiFrontend.ViewModels
{
    [QueryProperty(nameof(Keyword), "Keyword")]
    public partial class SearchResultsViewModel : ObservableObject
    {
        private readonly ProductService _productService;

        [ObservableProperty]
        private string keyword;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private ObservableCollection<Product> searchResults = new();

        [ObservableProperty]
        private int resultCount;

        public IRelayCommand GoBackCommand { get; }
        public IAsyncRelayCommand<string> GoToDetailCommand { get; }

        public SearchResultsViewModel(ProductService productService)
        {
            _productService = productService;
            GoBackCommand = new RelayCommand(async () => await Shell.Current.GoToAsync(".."));
            GoToDetailCommand = new AsyncRelayCommand<string>(GoToDetailAsync);
        }

        public async Task LoadResultsAsync()
        {
            if (string.IsNullOrWhiteSpace(Keyword))
                return;

            try
            {
                IsBusy = true;

                var resp = await _productService.SearchProductsAsync(Keyword, 20);

                if (resp?.Data != null)
                {
                    SearchResults = new ObservableCollection<Product>(resp.Data);
                    ResultCount = resp.Data.Count;
                }
                else
                {
                    SearchResults.Clear();
                    ResultCount = 0;
                }
            }
            finally
            {
                IsBusy = false;
            }
        }


        private async Task GoToDetailAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
                return;

            await Shell.Current.GoToAsync(nameof(ProductDetailPage), true, new Dictionary<string, object>
            {
                { "ProductID", productId }
            });
        }
    }
}
