using Microsoft.EntityFrameworkCore;
using Serilog;
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
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(
                    new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build())
                .CreateLogger();

            try
            {
                Log.Information("Starting SmartBank Account Service");

                var builder = WebApplication.CreateBuilder(args);

              builder.Host.UseSerilog();

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

                //builder.Services.AddHostedService<CustomerCreatedConsumer>();

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();

                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint(
                            "/swagger/v1/swagger.json",
                            "SmartBank Account Service v1");
                    });
                }

                app.UseMiddleware<ExceptionMiddleware>();

                app.UseHttpsRedirection();

                app.UseAuthorization();

                app.MapControllers();

                Console.WriteLine("Application started!");
                Console.WriteLine("Swagger: http://localhost:5242/swagger");

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}