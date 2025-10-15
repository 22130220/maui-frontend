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
        public string? ProductID { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("discountDefault")]
        public int DiscountDefault { get; set; }

        [JsonPropertyName("thumbnail")]
        public string? Thumbnail { get; set; }

        [JsonPropertyName("categoryID")]
        public string? CategoryID { get; set; }

        [JsonPropertyName("quantityStock")]
        public int QuantityStock { get; set; }

        [JsonPropertyName("quantitySell")]
        public int QuantitySell { get; set; }
    }
}
