using System.Text;
using System.Text.Json;

namespace Sales.Frontend.Repositories
{
    public class Repository : IRepository
    {

        private readonly HttpClient _httpClient;

        private JsonSerializerOptions _jsonSerializerOptions => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        public Repository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
       
        public async Task<HttpResponseWrapper<T>> GetAsync<T>(string url)
        {
            var responseHttp= await _httpClient.GetAsync(url);
            if(responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswerAsync<T>(responseHttp);
                return new HttpResponseWrapper<T>(response, false, responseHttp);

            }
            return new HttpResponseWrapper<T>(default, true, responseHttp);
        }

        

        public async Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model)
        {
            var messageJson=JsonSerializer.Serialize(model);
            var messangeContent = new StringContent(messageJson, Encoding.UTF8, "application/json");  
            var responseHttp=await _httpClient.PostAsync(url, messangeContent);
            return new HttpResponseWrapper<object>(null,!responseHttp.IsSuccessStatusCode , responseHttp);
        }

        public async Task<HttpResponseWrapper<TResponse>> PostAsync<T, TResponse>(string url, T model)
        {
            var messageJson = JsonSerializer.Serialize(model);
            var messangeContent = new StringContent(messageJson, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, messangeContent);
            if(responseHttp.IsSuccessStatusCode) 
            {
                var response = await UnserializeAnswerAsync<TResponse>(responseHttp);
                return new HttpResponseWrapper<TResponse> (response, false, responseHttp);
            }
            return new HttpResponseWrapper<TResponse>(default,true,responseHttp);
        }
        private async Task<T> UnserializeAnswerAsync<T>(HttpResponseMessage responseHttp)
        {
            var response=await responseHttp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(response, _jsonSerializerOptions)!;
        }
    }
}
