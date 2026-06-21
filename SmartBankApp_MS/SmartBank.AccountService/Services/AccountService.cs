using SmartBank.AccountService.DTOs;
using SmartBank.AccountService.Interfaces;
using SmartBank.AccountService.Models;

namespace SmartBank.AccountService.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            IAccountRepository repository,
            ILogger<AccountService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<AccountResponseDto>> GetAllAccountsAsync()
        {
            _logger.LogInformation("Fetching all accounts");

            var accounts = await _repository.GetAllAsync();

            return accounts.Select(a => new AccountResponseDto
            {
                AccountId = a.AccountId,
                AccountNumber = a.AccountNumber,
                AccountType = a.AccountType,
                Balance = a.Balance,
                IsActive = a.IsActive
            });
        }

        public async Task<AccountResponseDto?> GetAccountByIdAsync(int id)
        {
            _logger.LogInformation(
                "Fetching account with Id {AccountId}",
                id);

            var account = await _repository.GetByIdAsync(id);

            if (account == null)
            {
                _logger.LogWarning(
                    "Account not found with Id {AccountId}",
                    id);

                return null;
            }

            return new AccountResponseDto
            {
                AccountId = account.AccountId,
                AccountNumber = account.AccountNumber,
                AccountType = account.AccountType,
                Balance = account.Balance,
                IsActive = account.IsActive
            };
        }

        public async Task<IEnumerable<AccountResponseDto>>
            GetAccountsByCustomerIdAsync(int customerId)
        {
            _logger.LogInformation(
                "Fetching accounts for CustomerId {CustomerId}",
                customerId);

            var accounts =
                await _repository.GetByCustomerIdAsync(customerId);

            return accounts.Select(a => new AccountResponseDto
            {
                AccountId = a.AccountId,
                AccountNumber = a.AccountNumber,
                AccountType = a.AccountType,
                Balance = a.Balance,
                IsActive = a.IsActive
            });
        }

        public async Task CreateAccountAsync(CreateAccountDto dto)
        {
            _logger.LogInformation(
                "Creating account for CustomerId {CustomerId}",
                dto.CustomerId);

            var account = new Account
            {
                CustomerId = dto.CustomerId,
                AccountType = dto.AccountType,
                Balance = 0,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                AccountNumber =
                    "ACC" + DateTime.Now.Ticks.ToString().Substring(10)
            };

            await _repository.AddAsync(account);

            await _repository.SaveAsync();

            _logger.LogInformation(
                "Account created successfully for CustomerId {CustomerId}",
                dto.CustomerId);
        }

        public async Task<bool> CloseAccountAsync(int id)
        {
            _logger.LogInformation(
                "Closing account with Id {AccountId}",
                id);

            var account = await _repository.GetByIdAsync(id);

            if (account == null)
            {
                _logger.LogWarning(
                    "Account not found with Id {AccountId}",
                    id);

                return false;
            }

            account.IsActive = false;

            await _repository.UpdateAsync(account);

            await _repository.SaveAsync();

            _logger.LogInformation(
                "Account closed successfully with Id {AccountId}",
                id);

            return true;
        }
    }
}