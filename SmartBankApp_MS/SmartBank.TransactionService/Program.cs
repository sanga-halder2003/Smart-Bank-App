using Microsoft.EntityFrameworkCore;
using SmartBank.TransactionService.Data;
using SmartBank.TransactionService.Services;
using SmartBank.TransactionService.Messaging;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/transactionservice-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Host.UseSerilog();

// ✅ Add services
builder.Services.AddControllers();

// ✅ Database configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseDeveloperExceptionPage();
app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();