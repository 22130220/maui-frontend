using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiFrontend.Constants;
using MauiFrontend.Models;
using MauiFrontend.Services;
using MauiFrontend.Views;
using System.Collections.ObjectModel;

namespace MauiFrontend.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        private readonly ProductService _productService;

        private bool _isBusy;
        private ObservableCollection<Product> _searchSuggestions;
        private bool _isSuggestionsVisible;
        private string _searchText = string.Empty;
        private int _page = 1;
        private int _size = 10;
        private int _totalPages = 0;

        // ======= PROPERTIES =======
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public int Page
        {
            get => _page;
            set => SetProperty(ref _page, value);
        }

        public int Size
        {
            get => _size;
            set => SetProperty(ref _size, value);
        }

        public ObservableCollection<Product> ProductList { get; set; }

        public ObservableCollection<Product> SearchSuggestions
        {
            get => _searchSuggestions;
            set => SetProperty(ref _searchSuggestions, value);
        }

        public bool IsSuggestionsVisible
        {
            get => _isSuggestionsVisible;
            set => SetProperty(ref _isSuggestionsVisible, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    _ = OnSearchTextChangedAsync(value);
            }
        }

        // ======= COMMANDS =======
        public IAsyncRelayCommand LoadProductsCommand { get; }
        public IAsyncRelayCommand<string> GoToDetailCommand { get; }
        public IAsyncRelayCommand ToLoginPageCommand { get; }
        public IAsyncRelayCommand<Product> SuggestionTappedCommand { get; }

        public IRelayCommand HideSuggestionsCommand { get; }

        // ======= CONSTRUCTOR =======
        public HomePageViewModel(ProductService productService)
        {
            _productService = productService;

            ProductList = new ObservableCollection<Product>();
            _searchSuggestions = new ObservableCollection<Product>();

            LoadProductsCommand = new AsyncRelayCommand(GetProductListAsync);
            GoToDetailCommand = new AsyncRelayCommand<string>(GoToDetailAsync);
            ToLoginPageCommand = new AsyncRelayCommand(ToLoginPageAsync);
            SuggestionTappedCommand = new AsyncRelayCommand<Product>(OnSuggestionTappedAsync);

            HideSuggestionsCommand = new RelayCommand(() => IsSuggestionsVisible = false);
        }

        // ======= LOAD PRODUCT LIST =======
        private async Task GetProductListAsync()
        {
            if (IsBusy) return;
            if (_totalPages == Page) return;
            try
            {
                IsBusy = true;
                var productResp = await _productService
                    .GetSingleNoSeperatorAsync<ApiResponse<DataPaging<Product>>>(
                        APICONSTANT.PRODUCT.GET_LIST, $"?page={Page++}&size={Size}");

                if (productResp?.Data != null)
                {
                    await Task.Run(() =>
                    {
                        _totalPages = productResp?.Data.TotalPages ?? 0;
                        foreach (var p in productResp?.Data?.Content ?? [])
                            ProductList.Add(p);
                    });
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        // ======= SEARCH FUNCTION =======
        private async Task OnSearchTextChangedAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                IsSuggestionsVisible = false;
                SearchSuggestions.Clear();
                return;
            }

            // Gọi API search
            var resp = await _productService.SearchProductsAsync(keyword, 8);

            // ✅ Ghi log ra console (để kiểm tra dữ liệu trả về)
            if (resp != null)
            {
                Console.WriteLine($"[Search] Keyword: {keyword}");
                Console.WriteLine($"[Search] Code: {resp.Code}");
                Console.WriteLine($"[Search] Message: {resp.Message}");
                Console.WriteLine($"[Search] Data count: {resp.Data?.Count ?? 0}");

                if (resp.Data != null)
                {
                    foreach (var item in resp.Data)
                    {
                        Console.WriteLine($" - {item.ProductID} | {item.Name} | {item.Price} | {item.Thumbnail}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"[Search] Response null for keyword '{keyword}'");
            }

            // Cập nhật UI
            if (resp?.Data != null && resp.Data.Any())
            {
                SearchSuggestions = new ObservableCollection<Product>(resp.Data);
                IsSuggestionsVisible = true;
            }
            else
            {
                IsSuggestionsVisible = false;
                SearchSuggestions.Clear();
            }
        }


        // ======= CLICK ITEM GỢI Ý =======
        private async Task OnSuggestionTappedAsync(Product product)
        {
            if (product == null) return;
            Console.WriteLine($"[Tap] Product tapped: {product.ProductID} - {product.Name}");
            IsSuggestionsVisible = false;
            await GoToDetailAsync(product.ProductID);
        }

        // ======= DETAIL NAVIGATION =======
        private async Task GoToDetailAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
                return;

            await Shell.Current.GoToAsync(nameof(ProductDetailPage), true, new Dictionary<string, object>
            {
                { "ProductID", productId }
            });
        }

        // ======= LOGIN PAGE =======
        private async Task ToLoginPageAsync()
        {
            await Shell.Current.GoToAsync($"///LoginPage");
        }
    }
}
