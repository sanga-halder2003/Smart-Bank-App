using SmartBank.AuthService.Models;
namespace SmartBank.AuthService.Repositories
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<Role?> GetRoleByNameAsync(string roleName);
        Task AddUserAsync(User user);
        Task AddRoleAsync(Role role);
        Task SaveChangesAsync();
    }
}
