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

            builder.Services.AddControllers();

            builder.Services.AddDbContext<CustomerDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

     
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

            builder.Services.AddScoped<
                ICustomerService,
                SmartBank.CustomerServie.Services.CustomerService>();

            builder.Services.AddScoped<ICustomerPublisher, CustomerPublisher>();

            builder.Services.AddSingleton<RabbitMQConsumer>();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();


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
