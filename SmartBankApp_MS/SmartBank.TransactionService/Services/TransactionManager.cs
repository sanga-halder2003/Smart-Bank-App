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

        // ✅ DEPOSIT
        public async Task Deposit(DepositDto dto)
        {
            _context.Transactions.Add(new Transaction
            {
                AccountId = dto.AccountId,
                Amount = dto.Amount,
                Type = "Deposit"
            });

            await _context.SaveChangesAsync();

            // ✅ Publish Event
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
            _context.Transactions.Add(new Transaction
            {
                AccountId = dto.AccountId,
                Amount = dto.Amount,
                Type = "Withdraw"
            });

            await _context.SaveChangesAsync();

            // ✅ Publish Event
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
            _context.Transactions.Add(new Transaction
            {
                AccountId = dto.FromAccountId,
                DestinationAccountId = dto.ToAccountId,
                Amount = dto.Amount,
                Type = "Transfer"
            });

            await _context.SaveChangesAsync();

            // ✅ Publish Event
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

        // ✅ STATEMENT (READ ONLY - NO EVENT)
        public async Task<List<Transaction>> GetStatement(int accountId)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId || t.DestinationAccountId == accountId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}