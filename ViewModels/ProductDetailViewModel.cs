using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiFrontend.Models;
using MauiFrontend.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiFrontend.ViewModels
{
    [QueryProperty("ProductID", "ProductID")]
    public partial class ProductDetailViewModel : ObservableObject
    {
        private readonly ProductService _productService;
        private readonly CartService _cartService;
        private readonly ProductReviewService _productReviewService;

        [ObservableProperty]
        private string productID;

        [ObservableProperty]
        private ProductDetail product;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private int quantity = 1;

        [ObservableProperty]
        private bool canAddToCart = true;

        [ObservableProperty]
        private bool canIncrease = true;

        [ObservableProperty]
        private bool canDecrease = false;

        public ObservableCollection<ProductReview> TopFiveComment { get; set; }

        [ObservableProperty]
        private SummaryReview summaryReview = null;

        public IAsyncRelayCommand AddToCartCommand { get; }
        public IRelayCommand IncreaseQuantityCommand { get; }
        public IRelayCommand DecreaseQuantityCommand { get; }

        public IAsyncRelayCommand LoadSummaryReviewCommand {  get; }

        public IAsyncRelayCommand NavigateToProductReviewListCommand { get; }

        // Computed properties cho progress bar (0.0 - 1.0)
        public double StarCount5Progress => CalculateProgress(5);
        public double StarCount4Progress => CalculateProgress(4);
        public double StarCount3Progress => CalculateProgress(3);
        public double StarCount2Progress => CalculateProgress(2);
        public double StarCount1Progress => CalculateProgress(1);

        public string StarsDisplay
        {
            get
            {
                if (SummaryReview == null) return "☆☆☆☆☆";

                double rating = SummaryReview.AvgRating;
                int fullStars = (int)Math.Floor(rating);
                double fraction = rating - fullStars;
                int emptyStars = 5 - fullStars;

                string stars = new string('★', fullStars);

                // Half star
                if (fraction >= 0.3 && fraction < 0.8 && emptyStars > 0)
                {
                    stars += "⯨";
                    emptyStars--;
                }
                else if (fraction >= 0.8 && emptyStars > 0)
                {
                    stars += "★";
                    emptyStars--;
                }

                stars += new string('☆', emptyStars);

                return stars;
            }
        }

        public ProductDetailViewModel(ProductService productService, CartService cartService, ProductReviewService productReviewService)
        {
            _productService = productService;
            _cartService = cartService;
            _productReviewService = productReviewService;

            TopFiveComment = new ObservableCollection<ProductReview>();

            AddToCartCommand = new AsyncRelayCommand(AddToCartAsync, () => CanAddToCart);
            IncreaseQuantityCommand = new RelayCommand(IncreaseQuantity, () => CanIncrease);
            DecreaseQuantityCommand = new RelayCommand(DecreaseQuantity, () => CanDecrease);
            LoadSummaryReviewCommand = new AsyncRelayCommand(GetSummaryReviewAsync);
            NavigateToProductReviewListCommand = new AsyncRelayCommand(GoToProductReviewListPageAsync);
        }

        private double CalculateProgress(int star)
        {
            if (SummaryReview?.StarCount == null || SummaryReview.TotalComments == 0)
                return 0;

            if (SummaryReview.StarCount.TryGetValue(star.ToString(), out int count))
            {
                return (double)count / SummaryReview.TotalComments;
            }
            return 0;
        }

        partial void OnProductIDChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
                _ = LoadProductDetailAsync(value);
        }

        partial void OnQuantityChanged(int value)
        {
            UpdateButtonStates();
            System.Diagnostics.Debug.WriteLine($"[ProductDetailViewModel] Quantity changed to: {value}");
        }

        partial void OnProductChanged(ProductDetail value)
        {
            UpdateButtonStates();
        }

        private async Task LoadProductDetailAsync(string id)
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var result = await _productService.GetByIdAsync(id);
                if (result != null)
                {
                    Product = result;
                    Quantity = 1;
                    UpdateButtonStates();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void IncreaseQuantity()
        {
            if (Product != null && Quantity < Product.QuantityStock)
            {
                Quantity++;
                System.Diagnostics.Debug.WriteLine($"[ProductDetailViewModel] Increased to: {Quantity}");
            }
        }

        private void DecreaseQuantity()
        {
            if (Quantity > 1)
            {
                Quantity--;
                System.Diagnostics.Debug.WriteLine($"[ProductDetailViewModel] Decreased to: {Quantity}");
            }
        }

        private async Task AddToCartAsync()
        {
            if (Product == null || !CanAddToCart) return;

            _cartService.AddToCart(Product, Quantity);

            await Shell.Current.DisplayAlert(
                "Thành công",
                $"Đã thêm {Quantity} sản phẩm vào giỏ hàng!",
                "OK");

            Quantity = 1;
        }

        private void UpdateButtonStates()
        {
            CanAddToCart = Product != null &&
                          Quantity > 0 &&
                          Quantity <= Product.QuantityStock;

            canIncrease = Product != null && Quantity < Product.QuantityStock;
            canDecrease = Quantity > 1;

            AddToCartCommand.NotifyCanExecuteChanged();
            IncreaseQuantityCommand.NotifyCanExecuteChanged();
            DecreaseQuantityCommand.NotifyCanExecuteChanged();

            System.Diagnostics.Debug.WriteLine($"[ProductDetailViewModel] States - CanAdd: {CanAddToCart}, CanInc: {CanIncrease}, CanDec: {CanDecrease}");
        }

        private async Task GetSummaryReviewAsync()
        {
            var result = await _productReviewService.GetSummaryReviewAsync($"?sortByName=rating&sortOrder=desc&productId={ProductID}");
            if (result == null || result.Data == null) return;
            SummaryReview = result.Data;
            TopFiveComment.Clear();
            foreach(var review in SummaryReview.TopFiveComment)
            {
                TopFiveComment.Add(review);
            }
        }

        // Gọi OnPropertyChanged cho các progress khi SummaryReview thay đổi
        partial void OnSummaryReviewChanged(SummaryReview value)
        {
            OnPropertyChanged(nameof(StarCount5Progress));
            OnPropertyChanged(nameof(StarCount4Progress));
            OnPropertyChanged(nameof(StarCount3Progress));
            OnPropertyChanged(nameof(StarCount2Progress));
            OnPropertyChanged(nameof(StarCount1Progress));
            OnPropertyChanged(nameof(StarsDisplay));
        }

        private async Task GoToProductReviewListPageAsync()
        {
            if (string.IsNullOrEmpty(ProductID))
                return;

            await Shell.Current.GoToAsync(nameof(ProductReviewListPage), true, new Dictionary<string, object>
            {
                { "ProductID", ProductID }
            });
        }
    }
}