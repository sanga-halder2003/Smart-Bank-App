namespace SmartBank.AuthService.Messaging
{
    public interface IRabbitMQPublisher
    {
        Task PublishUserRegisteredEventAsync(UserRegisteredEvent userEvent);
    }
}
