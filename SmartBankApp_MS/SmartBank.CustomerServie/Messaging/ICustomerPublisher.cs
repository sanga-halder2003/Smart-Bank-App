namespace SmartBank.CustomerServie.Messaging
{
    public interface ICustomerPublisher
    {
        Task PublishCustomerCreatedEventAsync(
            CustomerCreatedEvent customerEvent);
    }
}