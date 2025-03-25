using System.Net.Http.Headers;
using System.Text.Json;

namespace api.Services
{
    public class UserInfoResponse
    {
        public string Sub { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        // Add other properties as needed
    }

    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AuthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<UserInfoResponse> GetUserInfoAsync(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", accessToken);

            var domain = _configuration["Auth0:Domain"];
            var response = await _httpClient.GetAsync(
                $"https://{domain}/userinfo"
            );

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserInfoResponse>(content, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
} 