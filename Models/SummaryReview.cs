using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiFrontend.Models
{
    public class SummaryReview : ObservableObject
    {
       
            [JsonPropertyName("avgRating")]
            public double AvgRating { get; set; }
            [JsonPropertyName("totalComments")]
            public int TotalComments { get; set; }
            [JsonPropertyName("starCount")]
            public Dictionary<string, int> StarCount { get; set; }
            [JsonPropertyName("topFiveComment")]
            public List<ProductReview> TopFiveComment { get; set; }
            [JsonPropertyName("hasComment")]
            public bool HasComment { get; set; } = true;
    }
    
}
