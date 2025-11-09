using MauiFrontend.Http;
using MauiFrontend.Models;
using MauiFrontend.Models.Request;
using MauiFrontend.Models.Response;
using System.Text.Json;

namespace MauiFrontend.Services
{
    public class CheckoutService
    {
        private readonly Https _https;
        private const string CHECKOUT_URL = "orders/checkout";

        public CheckoutService(Https https)
        {
            _https = https;
        }

        public async Task<ApiResponse<CheckoutResponse>> CheckoutAsync(CheckoutRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            try
            {
                var result = await _https.PostAsync<CheckoutRequest, ApiResponse<CheckoutResponse>>(
                    CHECKOUT_URL,
                    request
                );
                return result;
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CheckoutService] Network error: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CheckoutService] JSON deserialization error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CheckoutService] Unexpected error: {ex.Message}");
                throw;
            }
        }
    }
}
