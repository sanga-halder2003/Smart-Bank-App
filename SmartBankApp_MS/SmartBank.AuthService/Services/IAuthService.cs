using SmartBank.AuthService.DTOs;

namespace SmartBank.AuthService.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterRequestDto request);
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    }

   

    }
