using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiFrontend.Models
{
    public class Product
    {
        [JsonPropertyName("productID")]
        public string ProductID { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("discountDefault")]
        public int DiscountDefault { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("categoryID")]
        public Category Category { get; set; } = new Category();

        [JsonPropertyName("quanlityStock")]
        public int QuantityStock { get; set; }

        [JsonPropertyName("quanlitySell")]
        public int QuantitySell { get; set; }

        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; } = string.Empty;

        [JsonPropertyName("createAt")]
        public string? CreateAt { get; set; }

        [JsonPropertyName("minStockLevel")]
        public int MinStockLevel { get; set; }
    }
}
