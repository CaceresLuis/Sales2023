using Sales.Blazor.Repositories.Interfaces;
using Sales.Blazor.Responses;
using System.Text;
using System.Text.Json;

namespace Sales.Blazor.Repositories
{
    public class Repository : IRepository
    {
        private readonly HttpClient _httpClient;

        private JsonSerializerOptions _serializerOptions => new() { PropertyNameCaseInsensitive = true };

        public Repository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);
            if (!httpResponse.IsSuccessStatusCode)
                return new HttpResponseWrapper<T>(default, true, httpResponse);

            T response = await UnserializeAnswer<T>(httpResponse, _serializerOptions);
            return new HttpResponseWrapper<T>(response, false, httpResponse);
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T model)
        {
            string messageJson = JsonSerializer.Serialize(model);
            StringContent messageContent = new(messageJson, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = await _httpClient.PostAsync(url, messageContent);
            return new HttpResponseWrapper<object>(null, !httpResponse.IsSuccessStatusCode, httpResponse);
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T model)
        {
            string messageJson = JsonSerializer.Serialize(model);
            StringContent messageContent = new(messageJson, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = await _httpClient.PostAsync(url, messageContent);
            if (!httpResponse.IsSuccessStatusCode)
                return new HttpResponseWrapper<TResponse>(default, !httpResponse.IsSuccessStatusCode, httpResponse);
            
            var response = await UnserializeAnswer<TResponse>(httpResponse, _serializerOptions);
            return new HttpResponseWrapper<TResponse>(response, false, httpResponse);
        }

        private static async Task<T> UnserializeAnswer<T>(HttpResponseMessage HttpResponse, JsonSerializerOptions serializerOptions)
        {
            string stringResponse = await HttpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(stringResponse, serializerOptions);
        }
    }
}
