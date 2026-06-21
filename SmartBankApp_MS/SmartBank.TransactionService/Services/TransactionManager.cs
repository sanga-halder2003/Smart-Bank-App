using SmartBank.TransactionService.Data;
using SmartBank.TransactionService.DTOs;
using SmartBank.TransactionService.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartBank.TransactionService.Services
{
    public class TransactionManager
    {
        private readonly AppDbContext _context;

        public TransactionManager(AppDbContext context)
        {
            _context = context;
        }

        public async Task Deposit(DepositDto dto)
        {
            _context.Transactions.Add(new Transaction
            {
                AccountId = dto.AccountId,
                Amount = dto.Amount,
                Type = "Deposit"
            });

            await _context.SaveChangesAsync();
        }

        public async Task Withdraw(WithdrawDto dto)
        {
            _context.Transactions.Add(new Transaction
            {
                AccountId = dto.AccountId,
                Amount = dto.Amount,
                Type = "Withdraw"
            });

            await _context.SaveChangesAsync();
        }

        public async Task Transfer(TransferDto dto)
        {
            _context.Transactions.Add(new Transaction
            {
                AccountId = dto.FromAccountId,
                DestinationAccountId = dto.ToAccountId,
                Amount = dto.Amount,
                Type = "Transfer"
            });

            await _context.SaveChangesAsync();
        }

        public async Task<List<Transaction>> GetStatement(int accountId)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId || t.DestinationAccountId == accountId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}
