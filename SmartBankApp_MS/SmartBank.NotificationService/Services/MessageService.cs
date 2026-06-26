using SmartBank.NotificationService.DTOs;
using SmartBank.NotificationService.Repositories;

namespace SmartBank.NotificationService.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _repository;
        public MessageService(IMessageRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> SendMessageAsync(SMSNotificationDto notificationDto)
        {
            return _repository.SendMessageAsync(notificationDto);
        }
    }
}
