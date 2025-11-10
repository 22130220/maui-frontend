using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;


namespace MauiFrontend.Http
{
    public class Https
    {
        protected readonly HttpClient _httpClient;

        public Https(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        // GET generic
        public async Task<T?> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json);
        }

        // POST generic: gửi data kiểu TRequest, nhận về TResponse
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            string? token = Preferences.Get("auth_token", string.Empty);

            _httpClient.DefaultRequestHeaders.Authorization =
                       new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync(url, content);
            System.Diagnostics.Debug.WriteLine($"response nhận: {response}");

            response.EnsureSuccessStatusCode();

           
            // ✅ Log status code
            System.Diagnostics.Debug.WriteLine($"Status: {response.StatusCode}");

            var responseJson = await response.Content.ReadAsStringAsync();

            // ✅ Log raw JSON
            System.Diagnostics.Debug.WriteLine($"Raw JSON: {responseJson}");
            return JsonSerializer.Deserialize<TResponse>(responseJson);
        }

        // PUT generic
        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(responseJson);
        }

        // DELETE generic (có thể trả về object hoặc chỉ bool)
        public async Task<bool> DeleteAsync(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }

    }


}
