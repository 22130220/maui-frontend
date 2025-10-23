using MauiFrontend.Constants;
using MauiFrontend.Http;
using MauiFrontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiFrontend.Services
{
    public class ProductService : BaseService
    {

        public ProductService(Https https) : base(https)
        {
        }

        public async Task<ProductDetail?> GetByIdAsync(string id)
        {
            try
            {
                var url = $"{APICONSTANT.PRODUCT.DETAIL}{id}";
                var result = await GetSingleAsync<ApiResponse<ProductDetail>>(url);
                return result?.Data;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] GetByIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<ApiResponse<List<Product>>> SearchProductsAsync(string keyword, int? size = null)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new ApiResponse<List<Product>> { Code = 200, Data = new List<Product>() };

            string url = $"{APICONSTANT.PRODUCT.SEARCH}?keyword={Uri.EscapeDataString(keyword)}";
            if (size != null)
                url += $"&size={size}";

            var result = await GetSingleNoSeperatorAsync<ApiResponse<List<Product>>>(url);
            return result!;
        }
    }
}