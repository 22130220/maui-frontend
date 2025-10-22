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

                Console.WriteLine($"[HTTP] GET {url}");
                Console.WriteLine($"[HTTP] Response JSON: {JsonSerializer.Serialize(result)}");
                Console.WriteLine($"[HTTP] Response Result: {JsonSerializer.Serialize(result?.Data)}");

                return result?.Data;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] GetByIdAsync: {ex.Message}");
                return null;
            }
        }
    }
}