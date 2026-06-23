using SmartBank.AuthService.DTOs;
using SmartBank.AuthService.Messaging;
using SmartBank.AuthService.Models;
using SmartBank.AuthService.Repositories;

namespace SmartBank.AuthService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtService _jwtService;
        private readonly IRabbitMQPublisher _rabbitMQPublisher;
        public AuthService(IAuthRepository authRepository, IJwtService jwtService, IRabbitMQPublisher rabbitMQPublisher)
        {
            _authRepository = authRepository;
            _jwtService = jwtService;
            _rabbitMQPublisher = rabbitMQPublisher;

        }

        public async Task<string> RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _authRepository.GetUserByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return "User already exists";
            }

            var role = await _authRepository.GetRoleByNameAsync(request.RoleName);

            if (role == null)
            {
                role = new Role
                {
                    RoleName = request.RoleName
                };

                await _authRepository.AddRoleAsync(role);
                await _authRepository.SaveChangesAsync();
            }

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Password = request.Password,
                RoleId = role.Id
            };

            await _authRepository.AddUserAsync(user);
            await _authRepository.SaveChangesAsync();

            var userEvent = new UserRegisteredEvent
            {
                FullName = user.FullName,
                Email = user.Email,
                RoleName = role.RoleName
            };

            await _rabbitMQPublisher.PublishUserRegisteredEventAsync(userEvent);

            return "User registered successfully";
        }
        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await _authRepository.GetUserByEmailAsync(request.Email);

            if (user == null)
                return null;

            if (user.Password != request.Password)
                return null;

            var token = _jwtService.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                RoleName = user.Role?.RoleName ?? ""
            };
        }
    }
}