using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace SmartBank.CustomerServie.Messaging
{
    public class CustomerPublisher : ICustomerPublisher
    {
        private readonly IConfiguration _configuration;

        public CustomerPublisher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task PublishCustomerCreatedEventAsync(
            CustomerCreatedEvent customerEvent)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"],
                Port = int.Parse(_configuration["RabbitMQ:Port"]!),
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"]
            };

            using var connection =
                await factory.CreateConnectionAsync();

            using var channel =
                await connection.CreateChannelAsync();

            var queueName =
                _configuration["RabbitMQ:CustomerQueue"];

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var message =
                JsonSerializer.Serialize(customerEvent);

            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                body: body);
        }
    }
}