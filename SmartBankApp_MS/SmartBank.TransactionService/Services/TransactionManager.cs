using SmartBank.TransactionService.Data;
using SmartBank.TransactionService.DTOs;
using SmartBank.TransactionService.Models;
using SmartBank.TransactionService.Messaging;
using Microsoft.EntityFrameworkCore;

namespace SmartBank.TransactionService.Services
{
    public class TransactionManager
    {
        private readonly AppDbContext _context;
        private readonly IRabbitMQPublisher _publisher;

        public TransactionManager(AppDbContext context, IRabbitMQPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        // ✅ Calculate Balance
        private async Task<decimal> GetBalance(string accountId)
        {
            var transactions = await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .ToListAsync();

            decimal balance = 0;

            foreach (var t in transactions)
            {
                if (t.Type == "Deposit")
                    balance += t.Amount;

                else if (t.Type == "Withdraw")
                    balance -= t.Amount;

                else if (t.Type == "Transfer")
                    balance -= t.Amount;
            }

            return balance;
        }

        // ✅ DEPOSIT
        public async Task Deposit(DepositDto dto)
        {
            if (dto.Amount <= 0)
                throw new Exception("Amount must be greater than zero");

            _context.Transactions.Add(new Transaction
            {
                AccountId = dto.AccountId,
                Amount = dto.Amount,
                Type = "Deposit",
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            var eventMessage = new MoneyDepositedEvent
            {
                AccountId = dto.AccountId,
                Amount = dto.Amount,
                CreatedAt = DateTime.UtcNow
            };

            await _publisher.PublishMoneyDepositedEventAsync(eventMessage);
        }

        // ✅ WITHDRAW
        public async Task Withdraw(WithdrawDto dto)
        {
            if (dto.Amount <= 0)
                throw new Exception("Invalid withdraw amount");

            var balance = await GetBalance(dto.AccountId);

            if (balance < dto.Amount)
                throw new Exception("Balance cannot be negative");

            _context.Transactions.Add(new Transaction
            {
                AccountId = dto.AccountId,
                Amount = dto.Amount,
                Type = "Withdraw",
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            var eventMessage = new MoneyWithdrawnEvent
            {
                AccountId = dto.AccountId,
                Amount = dto.Amount,
                CreatedAt = DateTime.UtcNow
            };

            await _publisher.PublishMoneyWithdrawnEventAsync(eventMessage);
        }

        // ✅ TRANSFER
        public async Task Transfer(TransferDto dto)
        {
            if (dto.Amount <= 0)
                throw new Exception("Invalid transfer amount");

            if (dto.FromAccountId == dto.ToAccountId)
                throw new Exception("Cannot transfer to same account");

            var balance = await GetBalance(dto.FromAccountId);

            if (balance < dto.Amount)
                throw new Exception("Insufficient balance");

            if (string.IsNullOrWhiteSpace(dto.ToAccountId))
                throw new Exception("Invalid destination account");

            _context.Transactions.Add(new Transaction
            {
                AccountId = dto.FromAccountId,
                DestinationAccountId = dto.ToAccountId,
                Amount = dto.Amount,
                Type = "Transfer",
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            var eventMessage = new MoneyTransferredEvent
            {
                FromAccountId = dto.FromAccountId,
                ToAccountId = dto.ToAccountId,
                Amount = dto.Amount,
                CreatedAt = DateTime.UtcNow
            };

            await _publisher.PublishMoneyTransferredEventAsync(eventMessage);
        }

        // ✅ STATEMENT
        public async Task<List<Transaction>> GetStatement(string accountId)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId || t.DestinationAccountId == accountId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}