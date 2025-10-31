using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace MauiFrontend.Models
{
    public class CartItem : INotifyPropertyChanged
    {
        private int _quantity;

        [JsonPropertyName("productID")]
        public string ProductID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalPrice));
                    OnPropertyChanged(nameof(FormattedTotalPrice));
                    OnPropertyChanged(nameof(CanIncrease));
                }
            }
        }

        [JsonPropertyName("discountDefault")]
        public int DiscountDefault { get; set; }

        [JsonPropertyName("quantityStock")]
        public int QuantityStock { get; set; }

        [JsonIgnore]
        public decimal FinalPrice => Price * (1 - DiscountDefault / 100m);

        [JsonIgnore]
        public decimal TotalPrice => FinalPrice * Quantity;

        [JsonIgnore]
        public string FormattedPrice => $"{Price:N0} ₫";

        [JsonIgnore]
        public string FormattedFinalPrice => $"{FinalPrice:N0} ₫";

        [JsonIgnore]
        public string FormattedTotalPrice => $"{TotalPrice:N0} ₫";

        [JsonIgnore]
        public bool HasDiscount => DiscountDefault > 0;

        [JsonIgnore]
        public string DiscountText => HasDiscount ? $"-{DiscountDefault}%" : "";

        [JsonIgnore]
        public bool CanIncrease => Quantity < QuantityStock;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}