using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace SmartBank.AccountService.Messaging
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly IConfiguration _configuration;

        public RabbitMQPublisher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task PublishAccountCreatedEventAsync(
            AccountCreatedEvent accountEvent)
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
                _configuration["RabbitMQ:AccountQueue"];

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var message =
                JsonSerializer.Serialize(accountEvent);

            var body =
                Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                body: body);
        }
    }
}