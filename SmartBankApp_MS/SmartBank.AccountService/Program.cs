using Microsoft.EntityFrameworkCore;
using SmartBank.AccountService.Data;
using SmartBank.AccountService.Interfaces;
using SmartBank.AccountService.Repositories;
using SmartBank.AccountService.Services;
using SmartBank.AccountService.Middleware;
using SmartBank.AccountService.Messaging;

namespace SmartBank.AccountService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // ✅ FIX PORT (correct line)
            builder.WebHost.UseUrls("http://localhost:5242");

            // ✅ Services
            builder.Services.AddControllers();

            builder.Services.AddDbContext<AccountDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAccountService,
                SmartBank.AccountService.Services.AccountService>();

            builder.Services.AddScoped<IRabbitMQPublisher, RabbitMQPublisher>();

            builder.Services.AddHostedService<CustomerCreatedConsumer>();

            var app = builder.Build();

            // ✅ ✅ ALWAYS ENABLE SWAGGER
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Account Service API");
            });

            // ✅ Global exception middleware
            app.UseMiddleware<ExceptionMiddleware>();

            // ❌ REMOVE HTTPS
            // app.UseHttpsRedirection();

            // ✅ Test endpoint
            app.MapGet("/", () => "Account Service Running ✅");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}