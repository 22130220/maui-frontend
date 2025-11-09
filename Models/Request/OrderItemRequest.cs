using System.Text.Json.Serialization;

namespace MauiFrontend.Models.Request
{
    public class OrderItemRequest
    {
        [JsonPropertyName("productID")]
        public string productID { get; set; }

        [JsonPropertyName("quantity")]
        public int quantity { get; set; }

        [JsonPropertyName("totalPrice")]
        public long totalPrice { get; set; }
    }
}
