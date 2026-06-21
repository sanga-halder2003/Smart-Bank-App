using Microsoft.EntityFrameworkCore;
using SmartBank.AccountService.Models;

namespace SmartBank.AccountService.Data
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext(
            DbContextOptions<AccountDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(a => a.Balance)
                .HasPrecision(18, 2);
        }
    }
}