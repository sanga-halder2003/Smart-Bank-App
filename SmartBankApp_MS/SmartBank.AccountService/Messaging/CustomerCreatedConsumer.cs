using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using SmartBank.AccountService.Data;
using SmartBank.AccountService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace SmartBank.AccountService.Messaging
{
   public class CustomerCreatedConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public CustomerCreatedConsumer(
        IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "customer-to-account-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);

   consumer.ReceivedAsync += async (sender, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Customer Event Received: {message}");

    var customerEvent =
        JsonSerializer.Deserialize<CustomerCreatedEvent>(message);

    if (customerEvent != null)
    {
        using var scope = _scopeFactory.CreateScope();

        var dbContext =
            scope.ServiceProvider.GetRequiredService<AccountDbContext>();

        var account = new Account
        {
            CustomerId = customerEvent.CustomerId,
            AccountNumber = $"SB{DateTime.Now.Ticks}",
            AccountType = "Savings",
            Balance = 0,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        dbContext.Accounts.Add(account);

        await dbContext.SaveChangesAsync();

        Console.WriteLine(
            $"Account Created for CustomerId: {customerEvent.CustomerId}");
    }
};

            await channel.BasicConsumeAsync(
                queue: "customer-to-account-queue",
                autoAck: true,
                consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}