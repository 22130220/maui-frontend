
using System.Text.Json.Serialization;

namespace MauiFrontend.Models.Response
{
    public class CheckoutResponse
    {
        [JsonPropertyName("orderID")]
        public int orderID { get; set; }

        [JsonPropertyName("message")]
        public string message { get; set; }

        [JsonPropertyName("totalAmount")]
        public long totalAmount { get; set; }
    }
}