using System.Text;
using System.Text.Json;
using SmartBank.Frontend.Models.Auth;

namespace SmartBank.Frontend.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AuthService(HttpClient httpClient,
                           IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<HttpResponseMessage> RegisterAsync(RegisterRequest request)
        {
            var gateway = _configuration["Gateway:BaseUrl"];

            var json = JsonSerializer.Serialize(request);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            return await _httpClient.PostAsync(
                $"{gateway}/api/auth/register",
                content);
        }

        public async Task<HttpResponseMessage> LoginAsync(LoginRequest request)
        {
            var gateway = _configuration["Gateway:BaseUrl"];

            var json = JsonSerializer.Serialize(request);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            return await _httpClient.PostAsync(
                $"{gateway}/api/auth/login",
                content);
        }
    }
}