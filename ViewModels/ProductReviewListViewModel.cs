using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiFrontend.Models;
using MauiFrontend.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiFrontend.ViewModels
{
    [QueryProperty("ProductID", "ProductID")]
    public partial class ProductReviewListViewModel : ObservableObject
    {
        private readonly ProductReviewService productReviewService;

        private int _currentPage = 0;
        private const int PageSize = 3;
        private int maxPage = 99999;

        [ObservableProperty]
        private string productID;

        [ObservableProperty]
        private SummaryReview summaryReview;

        [ObservableProperty]
        private ObservableCollection<ProductReview> reviews = new();

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool hasMoreItems = true;

        [ObservableProperty]
        private int selectedStarFilter = 0; // 0 = All

        [ObservableProperty]
        private string selectedSortOrder = "Newest";

        // Computed properties for star distribution
        public double StarCount5Progress => CalculateProgress(5);
        public double StarCount4Progress => CalculateProgress(4);
        public double StarCount3Progress => CalculateProgress(3);
        public double StarCount2Progress => CalculateProgress(2);
        public double StarCount1Progress => CalculateProgress(1);

        public int StarCount5 => GetStarCount(5);
        public int StarCount4 => GetStarCount(4);
        public int StarCount3 => GetStarCount(3);
        public int StarCount2 => GetStarCount(2);
        public int StarCount1 => GetStarCount(1);

        // Computed property cho stars display
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

        public ProductReviewListViewModel(ProductReviewService productReviewService) 
        { 
            this.productReviewService = productReviewService;
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

        private int GetStarCount(int star)
        {
            if (SummaryReview?.StarCount == null)
                return 0;

            SummaryReview.StarCount.TryGetValue(star.ToString(), out int count);
            return count;
        }

        partial void OnSummaryReviewChanged(SummaryReview value)
        {
            OnPropertyChanged(nameof(StarsDisplay));
            OnPropertyChanged(nameof(StarCount5Progress));
            OnPropertyChanged(nameof(StarCount4Progress));
            OnPropertyChanged(nameof(StarCount3Progress));
            OnPropertyChanged(nameof(StarCount2Progress));
            OnPropertyChanged(nameof(StarCount1Progress));
            OnPropertyChanged(nameof(StarCount5));
            OnPropertyChanged(nameof(StarCount4));
            OnPropertyChanged(nameof(StarCount3));
            OnPropertyChanged(nameof(StarCount2));
            OnPropertyChanged(nameof(StarCount1));
        }

        [RelayCommand]
        public async Task LoadReviewsAsync()
        {
            var summary = await this.productReviewService.GetSummaryReviewAsync($"?sortByName=rating&sortOrder=desc&productId={ProductID}");
            SummaryReview = summary.Data;

            await LoadMoreReviewCommand.ExecuteAsync(null);
        }

        [RelayCommand]
        public void FilterByStar(string starString)
        {
            int star = int.Parse(starString);
            SelectedStarFilter = star;
            ResetPagination();
            LoadMoreReviewCommand.Execute(null);
        }

        [RelayCommand]
        public void ChangeSortOrder(string sortOrder)
        {
            if (SelectedSortOrder == sortOrder) return;

            SelectedSortOrder = sortOrder;
            ResetPagination();
            LoadMoreReviewCommand.Execute(null);
        }

        [RelayCommand]
        public async Task LoadMoreReview()
        {
            if (maxPage == _currentPage) return;
            IsLoading = true;

            var review = await this.productReviewService.GetListReview(BuildQuery());
            if (review != null)
            {
                foreach (var item in review.Data.Content)
                {
                    Reviews.Add(item);
                }

                maxPage = review.Data.TotalPages;
                _currentPage++;
            }

            IsLoading = false;
        }

        private void ResetPagination()
        {
            _currentPage = 0;
            maxPage = 9999;
            Reviews.Clear();
        }

        private string BuildQuery()
        {
            string sortName = selectedSortOrder switch
            {
                "Newest" => "createdAt",
                "Oldest" => "createdAt",
                "Highest" => "rating",
                "Lowest" => "rating",
                _ => "createdAt"
            };

            string sortOrder = selectedSortOrder switch
            {
                "Newest" => "desc",
                "Oldest" => "asc",
                "Highest" => "desc",
                "Lowest" => "asc",
                _ => "desc"
            };

            string? selectedStar = selectedStarFilter switch
            {
                0 => null,
                1 => "1",
                2 => "2",
                3 => "3",
                4 => "4",
                5 => "5",
                _ => null
            };

            var sb = new StringBuilder("?");
            sb.Append($"sortByName={sortName}&");
            sb.Append($"sortOrder={sortOrder}&");
            sb.Append($"productId={ProductID}&");
            sb.Append($"page={_currentPage}&");
            sb.Append($"size={PageSize}");

            if (selectedStar != null)
            {
                sb.Append($"&rating={selectedStar}");
            }

            string result = sb.ToString();
            return result;

        }
    }
}
