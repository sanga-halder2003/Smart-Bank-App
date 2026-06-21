using SmartBank.NotificationService.DTOs;

namespace SmartBank.NotificationService.Services
{
    public interface IMessageService
    {
        Task<bool> SendMessageAsync(SMSNotificationDto notificationDto);
    }
}
