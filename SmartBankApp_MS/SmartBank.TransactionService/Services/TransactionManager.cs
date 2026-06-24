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

        public TransactionManager(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Calculate Balance
        private async Task<decimal> GetBalance(int accountId)
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
                Type = "Deposit"
            });

            await _context.SaveChangesAsync();

            var publisher = new RabbitMQPublisher();

            var data = new
            {
                AccountId = dto.AccountId,
                Amount = dto.Amount,
                Type = "Deposit"
            };

            publisher.Publish(data, "money-deposited-queue");
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
                Type = "Withdraw"
            });

            await _context.SaveChangesAsync();

            var publisher = new RabbitMQPublisher();

            var data = new
            {
                AccountId = dto.AccountId,
                Amount = dto.Amount,
                Type = "Withdraw"
            };

            publisher.Publish(data, "money-withdrawn-queue");
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

            // ✅ Basic destination validation
            if (dto.ToAccountId <= 0)
                throw new Exception("Invalid destination account");

            _context.Transactions.Add(new Transaction
            {
                AccountId = dto.FromAccountId,
                DestinationAccountId = dto.ToAccountId,
                Amount = dto.Amount,
                Type = "Transfer"
            });

            await _context.SaveChangesAsync();

            var publisher = new RabbitMQPublisher();

            var data = new
            {
                FromAccountId = dto.FromAccountId,
                ToAccountId = dto.ToAccountId,
                Amount = dto.Amount,
                Type = "Transfer"
            };

            publisher.Publish(data, "money-transferred-queue");
        }

        // ✅ STATEMENT (READ ONLY)
        public async Task<List<Transaction>> GetStatement(int accountId)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId || t.DestinationAccountId == accountId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}