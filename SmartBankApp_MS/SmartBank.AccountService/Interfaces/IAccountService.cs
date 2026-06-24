using SmartBank.AccountService.DTOs;

namespace SmartBank.AccountService.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountResponseDto>> GetAllAccountsAsync();

        Task<AccountResponseDto?> GetAccountByIdAsync(int id);

        Task<IEnumerable<AccountResponseDto>> GetAccountsByCustomerIdAsync(int customerId);

        Task CreateAccountAsync(CreateAccountDto dto);

        Task<bool> CloseAccountAsync(int id);
    }
}