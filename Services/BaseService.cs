using MauiFrontend.Http;
using MauiFrontend.Models;
using System.Text.Json;

namespace MauiFrontend.Services
{
    public class BaseService
    {
        protected readonly Https _https;

        public BaseService(Https https)
        {
            _https = https;
        }

        public async Task<T?> GetSingleAsync<T>(string url, string param = "")
        {
            var realUrl = $"{BaseService.LastSeparator(url)}{param}";
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


    

        public async Task<List<T>> GetListAsync<T>(string url, string param = "")
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

        public async Task<T?> GetSingleNoSeperatorAsync<T>(string url, string param = "")
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

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            try
            {
                var result = await _https.PostAsync<TRequest, TResponse>(url, data);
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

        private static string LastSeparator(string url)
        {
            if (!url.EndsWith('/'))
            {
                url += "/";
            }
            return url;
        }
    }
}
