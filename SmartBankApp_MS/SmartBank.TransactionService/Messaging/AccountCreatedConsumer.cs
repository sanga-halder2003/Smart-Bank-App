using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace SmartBank.TransactionService.Messaging
{
    public class AccountCreatedConsumer : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            // ✅ Listen to Account → Transaction queue
            channel.QueueDeclare(
                queue: "account-to-transaction-queue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"✅ Account Event Received: {message}");

                // 👉 You can process it here later
            };

            channel.BasicConsume(
                queue: "account-to-transaction-queue",
                autoAck: true,
                consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
