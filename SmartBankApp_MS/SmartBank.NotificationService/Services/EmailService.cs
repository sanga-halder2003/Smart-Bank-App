using SmartBank.NotificationService.DTOs;
using SmartBank.NotificationService.Repositories;

namespace SmartBank.NotificationService.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository _repository;
        public EmailService(IEmailRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> SendEmailAsync(EmailNotificationDto notification)
        {
            return _repository.SendEmailAsync(notification);
        }
    }
}
