using SmartBank.NotificationService.DTOs;

namespace SmartBank.NotificationService.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailNotificationDto notificationDto);
    }
}
