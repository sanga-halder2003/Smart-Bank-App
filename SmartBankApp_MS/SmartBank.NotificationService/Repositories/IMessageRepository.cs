using SmartBank.NotificationService.DTOs;

namespace SmartBank.NotificationService.Repositories
{
    public interface IMessageRepository
    {
        Task<bool> SendMessageAsync(SMSNotificationDto notificationDto);
    }
}
