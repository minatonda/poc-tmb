using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Hubs;
using OrderApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<OrderService>();

// Configure CORS para permitir seu frontend (ajuste URL)
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
        policy.WithOrigins("http://localhost:4200") // URL do seu frontend React
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()); // Importante para SignalR
});

builder.Services.AddSignalR();

var app = builder.Build();

// Use CORS antes do MapHub e MapControllers
app.UseCors("CorsPolicy");

app.MapHub<OrderHub>("/hub/order");

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

// Escuta em 0.0.0.0:5000 para Docker
app.Run("http://0.0.0.0:5000");
