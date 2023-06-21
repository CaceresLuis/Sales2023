using System.Text;
using System.Text.Json;
using Sales.Web.Responses;
using System.Net.Http.Json;
using Sales.Web.Repositories.Interfaces;
using Sales.Shared.Responses;

namespace Sales.Web.Repositories
{
    public class Repository : IRepository
    {
        private readonly HttpClient _httpClient;

        private JsonSerializerOptions _jsonSerializerOptions => new() { PropertyNameCaseInsensitive = true };

        public Repository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);
            if (!httpResponse.IsSuccessStatusCode)
                return new HttpResponseWrapper<T>(default, true, httpResponse);

            T response = await UnserializeAnswer<T>(httpResponse, _jsonSerializerOptions);
            return new HttpResponseWrapper<T>(response, false, httpResponse);
        }

        public async Task<HttpResponseWrapper<object>> Get(string url)
        {
            var httpResponse = await _httpClient.GetAsync(url);
            return new HttpResponseWrapper<object>(null, !httpResponse.IsSuccessStatusCode, httpResponse);
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T model)
        {
            HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync(url, model);
            return new HttpResponseWrapper<object>(null, !httpResponse.IsSuccessStatusCode, httpResponse);
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T model)
        {
            HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync(url, model);
            if (!httpResponse.IsSuccessStatusCode)
                return new HttpResponseWrapper<TResponse>(default, !httpResponse.IsSuccessStatusCode, httpResponse);

            TResponse? response = await UnserializeAnswer<TResponse>(httpResponse, _jsonSerializerOptions);
            return new HttpResponseWrapper<TResponse>(response, false, httpResponse);
        }

        public async Task<HttpResponseWrapper<object>> Put<T>(string url, T model)
        {
            HttpResponseMessage httpResponse = await _httpClient.PutAsJsonAsync(url, model);
            return new HttpResponseWrapper<object>(null, !httpResponse.IsSuccessStatusCode, httpResponse);
        }

        public async Task<HttpResponseWrapper<TResponse>> Put<T, TResponse>(string url, T model)
        {
            HttpResponseMessage httpResponse = await _httpClient.PutAsJsonAsync(url, model);
            if (!httpResponse.IsSuccessStatusCode)
                return new HttpResponseWrapper<TResponse>(default, !httpResponse.IsSuccessStatusCode, httpResponse);

            TResponse? response = await UnserializeAnswer<TResponse>(httpResponse, _jsonSerializerOptions);
            return new HttpResponseWrapper<TResponse>(response, false, httpResponse);
        }

        public async Task<HttpResponseWrapper<object>> Delete(string url)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(url);
            return new HttpResponseWrapper<object>(null, !response.IsSuccessStatusCode, response);
        }

        private static async Task<T> UnserializeAnswer<T>(HttpResponseMessage HttpResponse, JsonSerializerOptions serializerOptions)
        {
            string stringResponse = await HttpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(stringResponse, serializerOptions)!;
        }
    }
}
