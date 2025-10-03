using MauiFrontend.Http;
using System.Text.Json;

namespace MauiFrontend.Services
{
    public class BaseService<T>
    {
        private readonly Https _https;

        public BaseService(Https https)
        {
            _https = https;
        }

        public async Task<T?> GetSingleAsync(string url, string param)
        {
            var realUrl = $"{LastSeparator(url)}{param}";
            try
            {
                var result = await _https.GetAsync<T>(realUrl);
                return result;
            }
            catch (HttpRequestException ex)
            {
                //TODO: Lỗi mạng, server
            }
            catch (JsonException ex)
            {
                //TODO: Lỗi deserialize
            }
            catch (Exception ex)
            {
                //TODO: Lỗi khác
            }

            return default;
        }
    

        public async Task<List<T>> GetListAsync(string url, string param = "")
        {
            var realUrl = $"{url}{param}";
            try
            {
                var result = await _https.GetAsync<List<T>>(realUrl);
                return result;
            }
            catch (HttpRequestException ex)
            {
                //TODO: Lỗi mạng, server
            }
            catch (JsonException ex)
            {
                //TODO: Lỗi deserialize
            }
            catch (Exception ex)
            {
                //TODO: Lỗi khác
            }

            return default;
        }

        public async Task<T?> GetSingleNoSeperatorAsync(string url, string param)
        {
            var realUrl = $"{url}{param}";
            try
            {
                var result = await _https.GetAsync<T>(realUrl);
                return result;
            }
            catch (HttpRequestException ex)
            {
                //TODO: Lỗi mạng, server
            }
            catch (JsonException ex)
            {
                //TODO: Lỗi deserialize
            }
            catch (Exception ex)
            {
                //TODO: Lỗi khác
            }

            return default;
        }


        private string LastSeparator(string url)
        {
            if (!url.EndsWith('/'))
            {
                url += "/";
            }
            return url;
        }
    }
}
