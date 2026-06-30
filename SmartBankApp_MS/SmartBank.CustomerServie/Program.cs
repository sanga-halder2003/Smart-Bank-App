using Microsoft.EntityFrameworkCore;
using Serilog;
using SmartBank.CustomerServie.Data;
using SmartBank.CustomerServie.Messaging;
using SmartBank.CustomerServie.Middleware;
using SmartBank.CustomerServie.Repositories;
using SmartBank.CustomerServie.Services;

namespace SmartBank.CustomerServie
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            // Add Services
            builder.Services.AddControllers();

            builder.Services.AddDbContext<CustomerDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // Dependency Injection
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

            builder.Services.AddScoped<
                ICustomerService,
                SmartBank.CustomerServie.Services.CustomerService>();

            builder.Services.AddScoped<
                ICustomerPublisher,
                CustomerPublisher>();

            builder.Services.AddSingleton<RabbitMQConsumer>();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Global Exception Middleware
            app.UseCustomExceptionMiddleware();

            // Start RabbitMQ Consumer
            var consumer = app.Services.GetRequiredService<RabbitMQConsumer>();
            await consumer.StartConsumingAsync();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
