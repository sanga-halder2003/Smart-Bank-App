using Microsoft.EntityFrameworkCore;
using SmartBank.CustomerServie.Data;
using SmartBank.CustomerServie.Repositories;
using SmartBank.CustomerServie.Services;
using SmartBank.CustomerServie.Messaging;

namespace SmartBank.CustomerServie
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ✅ FIX PORT
            builder.WebHost.UseUrls("http://localhost:5098");

            // ✅ Services
            builder.Services.AddControllers();

            builder.Services.AddDbContext<CustomerDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // ✅ DI
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ICustomerService,
                SmartBank.CustomerServie.Services.CustomerService>();
            builder.Services.AddScoped<ICustomerPublisher, CustomerPublisher>();

            builder.Services.AddSingleton<RabbitMQConsumer>();

            // ✅ Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // ✅ Start RabbitMQ consumer
            var consumer = app.Services.GetRequiredService<RabbitMQConsumer>();
            await consumer.StartConsumingAsync();

            // ✅ ✅ ALWAYS ENABLE SWAGGER
            app.UseSwagger();
            app.UseSwaggerUI();

            // ❌ REMOVE HTTPS (important for your case)
            // app.UseHttpsRedirection();

            // ✅ Test endpoint
            app.MapGet("/", () => "Customer Service Running ✅");

            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}