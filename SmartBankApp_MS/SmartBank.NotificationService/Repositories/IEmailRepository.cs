using SmartBank.NotificationService.DTOs;

namespace SmartBank.NotificationService.Repositories
{
    public interface IEmailRepository
    {
        Task<bool> SendEmailAsync(EmailNotificationDto notification);
    }
}
