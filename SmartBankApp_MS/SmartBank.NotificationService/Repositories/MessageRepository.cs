using SmartBank.NotificationService.DTOs;

namespace SmartBank.NotificationService.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        public async Task<bool> SendMessageAsync(SMSNotificationDto notificationDto)
        {
            try
            {
                await Task.Run(() =>
                {
                    Console.WriteLine("=================================");
                    Console.WriteLine("SMS SENT");
                    Console.WriteLine($"To      : {notificationDto.Recipient}");
                    Console.WriteLine($"Message : {notificationDto.Message}");
                    Console.WriteLine("=================================");
                });

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SMS Error: {ex.Message}");
                return false;
            }
        }
    }
}
