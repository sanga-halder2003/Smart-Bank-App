using System.Net.Http.Json;
using SmartBank.Frontend.Models.Transaction;

namespace SmartBank.Frontend.Services;

public class TransactionService
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    public TransactionService(HttpClient client,
        IConfiguration configuration)
    {
        _client = client;
        _configuration = configuration;
    }

    private string BaseUrl =>
        $"{_configuration["Gateway:BaseUrl"]}/api/transaction";

    public async Task DepositAsync(DepositDto dto)
    {
        await _client.PostAsJsonAsync($"{BaseUrl}/deposit", dto);
    }

    public async Task WithdrawAsync(WithdrawDto dto)
    {
        await _client.PostAsJsonAsync($"{BaseUrl}/withdraw", dto);
    }

    public async Task TransferAsync(TransferDto dto)
    {
        await _client.PostAsJsonAsync($"{BaseUrl}/transfer", dto);
    }

    public async Task<List<TransactionModel>> GetStatementAsync(int accountId)
    {
        return await _client.GetFromJsonAsync<List<TransactionModel>>
        ($"{BaseUrl}/statement/{accountId}")
        ?? new();
    }
}