
using SmartBank.NotificationService.Repositories;
using SmartBank.NotificationService.Services;

namespace SmartBank.NotificationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add services to the container.
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IMessageService, MessageService>();

            builder.Services.AddScoped<IEmailRepository, EmailRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
