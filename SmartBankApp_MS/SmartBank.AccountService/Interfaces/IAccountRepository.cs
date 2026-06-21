using SmartBank.AccountService.Models;

namespace SmartBank.AccountService.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAsync();

        Task<Account?> GetByIdAsync(int id);

        Task<IEnumerable<Account>> GetByCustomerIdAsync(int customerId);

        Task AddAsync(Account account);

        Task UpdateAsync(Account account);

        Task SaveAsync();
    }
}