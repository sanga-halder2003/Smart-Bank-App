using Microsoft.EntityFrameworkCore;
using SmartBank.AuthService.Models;

namespace SmartBank.AuthService.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext>options): base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

    }
}
