using Microsoft.EntityFrameworkCore;
using SmartBank.TransactionService.Data;
using SmartBank.TransactionService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

// ✅ THIS IS THE FIX
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=TransactionDB;Trusted_Connection=True;TrustServerCertificate=True;"));
builder.Services.AddScoped<TransactionManager>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();