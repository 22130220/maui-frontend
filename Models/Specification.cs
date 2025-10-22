using System.Text.Json.Serialization;

namespace MauiFrontend.Models
{
    public class Specification
    {
        [JsonPropertyName("specificationID")]
        public string? SpecificationID { get; set; }

        [JsonPropertyName("dimensions")]
        public string? Dimensions { get; set; }

        [JsonPropertyName("material")]
        public string? Material { get; set; }

        [JsonPropertyName("original")]
        public string? Original { get; set; }

        [JsonPropertyName("standard")]
        public string? Standard { get; set; }

        [JsonPropertyName("productID")]
        public string? ProductID { get; set; }
    }
}