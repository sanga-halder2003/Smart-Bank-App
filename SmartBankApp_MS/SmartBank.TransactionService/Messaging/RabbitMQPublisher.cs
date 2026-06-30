using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartBank.TransactionService.Messaging
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        // ✅ All methods now publish to SAME queue

        public Task PublishMoneyDepositedEventAsync(MoneyDepositedEvent request)
        {
            var message = new
            {
                Type = "Deposit",
                Data = request
            };

            return PublishAsync("account-to-transaction-queue", message);
        }

        public Task PublishMoneyWithdrawnEventAsync(MoneyWithdrawnEvent request)
        {
            var message = new
            {
                Type = "Withdraw",
                Data = request
            };

            return PublishAsync("account-to-transaction-queue", message);
        }

        public Task PublishMoneyTransferredEventAsync(MoneyTransferredEvent request)
        {
            var message = new
            {
                Type = "Transfer",
                Data = request
            };

            return PublishAsync("account-to-transaction-queue", message);
        }

        private Task PublishAsync(string queueName, object obj)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // ✅ Declare ONE queue
            channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // ✅ Serialize message with Type info
            var message = JsonSerializer.Serialize(obj);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body);

            return Task.CompletedTask;
        }
    }
}
