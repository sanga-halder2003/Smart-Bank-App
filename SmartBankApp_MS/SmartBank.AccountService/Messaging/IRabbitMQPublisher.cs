namespace SmartBank.AccountService.Messaging
{
    public interface IRabbitMQPublisher
    {
        Task PublishAccountCreatedEventAsync(
            AccountCreatedEvent accountEvent);
    }
}