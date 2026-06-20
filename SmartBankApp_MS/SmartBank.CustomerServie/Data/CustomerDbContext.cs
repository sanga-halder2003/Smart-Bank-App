using Microsoft.EntityFrameworkCore;
using SmartBank.CustomerServie.Models;

namespace SmartBank.CustomerServie.Data
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
    }
}