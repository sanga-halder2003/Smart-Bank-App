using System.Net.Http.Json;
using SmartBank.Frontend.Models.Account;

namespace SmartBank.Frontend.Services;

public class AccountService
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    public AccountService(HttpClient client,
        IConfiguration configuration)
    {
        _client = client;
        _configuration = configuration;
    }

    private string BaseUrl =>
        $"{_configuration["Gateway:BaseUrl"]}/api/account";

    public async Task<List<AccountModel>> GetAllAsync()
    {
        return await _client.GetFromJsonAsync<List<AccountModel>>(BaseUrl)
               ?? new();
    }

    public async Task<AccountModel?> GetByIdAsync(int id)
    {
        return await _client.GetFromJsonAsync<AccountModel>($"{BaseUrl}/{id}");
    }

    public async Task<HttpResponseMessage> CreateAsync(CreateAccountDto dto)
    {
        return await _client.PostAsJsonAsync(BaseUrl, dto);
    }

    public async Task<HttpResponseMessage> CloseAsync(int id)
    {
        return await _client.PutAsync($"{BaseUrl}/close/{id}", null);
    }
}