using System.Text;
using System.Text.Json.Serialization;

namespace MauiFrontend.Models
{
    public class ProductDetail
    {
        [JsonPropertyName("productID")]
        public string? ProductID { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("discountDefault")]
        public int DiscountDefault { get; set; }

        [JsonPropertyName("thumbnail")]
        public string? Thumbnail { get; set; }

        [JsonPropertyName("categoryID")]
        public Category? Category { get; set; }

        [JsonPropertyName("quantityStock")]
        public int QuantityStock { get; set; }

        [JsonPropertyName("quantitySell")]
        public int QuantitySell { get; set; }

        [JsonPropertyName("specification")]
        public Specification? Specification { get; set; }

        // ➕ Thêm thuộc tính này để hiển thị nội dung trong XAML
        [JsonIgnore]
        public string SpecificationsText
        {
            get
            {
                if (Specification == null) return "Không có thông số kỹ thuật.";

                var sb = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(Specification.Material))
                    sb.AppendLine($"• Chất liệu: {Specification.Material}");
                if (!string.IsNullOrWhiteSpace(Specification.Dimensions))
                    sb.AppendLine($"• Kích thước: {Specification.Dimensions}");
                if (!string.IsNullOrWhiteSpace(Specification.Standard))
                    sb.AppendLine($"• Tiêu chuẩn: {Specification.Standard}");
                if (!string.IsNullOrWhiteSpace(Specification.Original))
                    sb.AppendLine($"• Xuất xứ: {Specification.Original}");

                return sb.ToString().Trim();
            }
        }
    }
}
