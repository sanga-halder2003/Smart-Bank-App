using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SmartBank.CustomerServie.Models;
using SmartBank.CustomerServie.Repositories;
using System.Text;
using System.Text.Json;

namespace SmartBank.CustomerServie.Messaging
{
    public class RabbitMQConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMQConsumer(
            IConfiguration configuration,
            IServiceScopeFactory scopeFactory)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
        }

        public async Task StartConsumingAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"],
                Port = int.Parse(_configuration["RabbitMQ:Port"]!),
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"]
            };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            var queueName = _configuration["RabbitMQ:QueueName"];

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var userEvent =
                    JsonSerializer.Deserialize<UserRegisteredEvent>(message);

                if (userEvent != null)
                {
                    using var scope = _scopeFactory.CreateScope();

                    var repository =
                        scope.ServiceProvider
                             .GetRequiredService<ICustomerRepository>();

                    var publisher =
                        scope.ServiceProvider
                             .GetRequiredService<ICustomerPublisher>();

                    var customer = new Customer
                    {
                        FullName = userEvent.FullName,
                        Email = userEvent.Email,
                        Phone = "",
                        Address = ""
                    };

                    var savedCustomer =
                        await repository.AddAsync(customer);

                    await publisher.PublishCustomerCreatedEventAsync(
                        new CustomerCreatedEvent
                        {
                            CustomerId = savedCustomer.Id,
                            FullName = savedCustomer.FullName,
                            Email = savedCustomer.Email,
                            Phone = savedCustomer.Phone,
                            Address = savedCustomer.Address
                        });
                }
            };

            await channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: true,
                consumer: consumer);
        }
    }
}