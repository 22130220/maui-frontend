using System.Text.Json.Serialization;

namespace MauiFrontend.Models.Request
{
    public class CheckoutRequest
    {
        [JsonPropertyName("fullname")]
        public string fullname { get; set; }

        [JsonPropertyName("email")]
        public string email { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string phoneNumber { get; set; }

        [JsonPropertyName("address")]
        public string address { get; set; }

        [JsonPropertyName("items")]
        public List<OrderItemRequest> items { get; set; }

        [JsonPropertyName("fee")]
        public int fee { get; set; } = 0;

        [JsonPropertyName("discount")]
        public int discount { get; set; } = 0;

        [JsonPropertyName("note")]
        public string note { get; set; } = "";
    }
}