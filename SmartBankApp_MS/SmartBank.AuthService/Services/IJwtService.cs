using SmartBank.AuthService.Models;

namespace SmartBank.AuthService.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}