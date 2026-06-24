using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace SmartBank.TransactionService.Messaging
{
    public class RabbitMQPublisher
    {
        public void Publish(object obj, string queueName)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // ✅ Declare dynamic queue
            channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // ✅ Serialize message
            var message = JsonSerializer.Serialize(obj);
            var body = Encoding.UTF8.GetBytes(message);

            // ✅ Publish to specific queue
            channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body);
        }
    }
}
