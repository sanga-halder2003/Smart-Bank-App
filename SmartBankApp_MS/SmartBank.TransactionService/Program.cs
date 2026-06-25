using Microsoft.EntityFrameworkCore;
using SmartBank.TransactionService.Data;
using SmartBank.TransactionService.Services;
using SmartBank.TransactionService.Messaging;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add services
builder.Services.AddControllers();

// ✅ Database configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        "Server=localhost\\SQLEXPRESS;Database=TransactionDB;Trusted_Connection=True;TrustServerCertificate=True;"
    ));

// ✅ Register Transaction Service
builder.Services.AddScoped<TransactionManager>();

// ✅ ✅ Register RabbitMQ Publisher
builder.Services.AddScoped<IRabbitMQPublisher, RabbitMQPublisher>();

// ✅ ✅ REGISTER CONSUMER (VERY IMPORTANT FOR FLOW)
builder.Services.AddHostedService<AccountCreatedConsumer>();

// ✅ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Middleware
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();