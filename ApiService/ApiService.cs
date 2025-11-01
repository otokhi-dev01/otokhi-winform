using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApiService
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private string _baseUrl;
        private string _token;

        public ApiService(string baseUrl)
        {
            _baseUrl = baseUrl.TrimEnd('/');
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Login and store bearer token.
        /// </summary>
        public async Task<bool> LoginAsync(string username, string password)
        {
            var payload = new
            {
                Username = username,
                Password = password
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/user/login", content);

            if (!response.IsSuccessStatusCode)
                return false;

            var json = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(json);

            _token = result?.token;  // assumes response { "token": "xxx" }

            if (!string.IsNullOrEmpty(_token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Send GET request with authorization
        /// </summary>
        public async Task<string> GetAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Send POST request with authorization
        /// </summary>
        public async Task<string> PostAsync(string endpoint, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/{endpoint}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Send PUT request with authorization
        /// </summary>
        public async Task<string> PutAsync(string endpoint, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}/{endpoint}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Send DELETE request with authorization
        /// </summary>
        public async Task<string> DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{endpoint}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public bool IsLoggedIn => !string.IsNullOrEmpty(_token);
    }
}
