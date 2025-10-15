using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiFrontend.Models
{
    public class DataPaging<T>
    {
        [JsonPropertyName("content")]
        public List<T>? Content { get; set; }

        [JsonPropertyName("pageable")]
        public Pageable? Pageable { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("totalElements")]
        public int TotalElements { get; set; }

        [JsonPropertyName("last")]
        public bool Last { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("sort")]
        public Sort? Sort { get; set; }

        [JsonPropertyName("numberOfElements")]
        public int NumberOfElements { get; set; }

        [JsonPropertyName("first")]
        public bool First { get; set; }

        [JsonPropertyName("empty")]
        public bool Empty { get; set; }
    }

    public class Pageable
    {
        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("sort")]
        public Sort? Sort { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("unpaged")]
        public bool Unpaged { get; set; }

        [JsonPropertyName("paged")]
        public bool Paged { get; set; }
    }

    public class Sort
    {
        [JsonPropertyName("empty")]
        public bool Empty { get; set; }

        [JsonPropertyName("unsorted")]
        public bool Unsorted { get; set; }

        [JsonPropertyName("sorted")]
        public bool Sorted { get; set; }
    }
}
