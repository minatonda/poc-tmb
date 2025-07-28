using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Azure.Messaging.ServiceBus;
using System.Text.Json;
using OrderApi.Data;
using OrderApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System;

namespace OrderWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private ServiceBusProcessor _processor;
        private readonly HttpClient _httpClient;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _httpClient = new HttpClient(); // HttpClient para notificação
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var client = new ServiceBusClient(_configuration["ServiceBus:ConnectionString"]);
            _processor = client.CreateProcessor(_configuration["ServiceBus:QueueName"], new ServiceBusProcessorOptions());
            _processor.ProcessMessageAsync += ProcessMessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;
            await _processor.StartProcessingAsync(cancellationToken);
            _logger.LogInformation("ServiceBusProcessor started.");
        }

        private async Task NotifyStatusChangeAsync(Guid orderId, string newStatus)
        {
            var notification = new
            {
                OrderId = orderId.ToString(),
                NewStatus = newStatus
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("http://orderapi:5000/api/notifications/order-status-changed", notification);
                response.EnsureSuccessStatusCode();
                _logger.LogInformation($"Notificação enviada para order {orderId} com status {newStatus}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao notificar backend via HTTP");
            }
        }

        private async Task ProcessMessageHandler(ProcessMessageEventArgs args)
        {
            var body = args.Message.Body.ToString();
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(body);
            if (data == null || !data.TryGetValue("OrderId", out var orderIdStr) || !Guid.TryParse(orderIdStr, out var orderId))
                return;

            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
            var order = await db.Orders.FindAsync(orderId);
            if (order == null) return;

            _logger.LogInformation($"[ServiceBus] Processando pedido {order.Id}");
            order.Status = OrderStatus.Processando;
            await db.SaveChangesAsync();
            await NotifyStatusChangeAsync(order.Id, order.Status.ToString());

            await Task.Delay(5000);

            order.Status = OrderStatus.Finalizado;
            await db.SaveChangesAsync();
            _logger.LogInformation($"[ServiceBus] Pedido {order.Id} finalizado.");
            await NotifyStatusChangeAsync(order.Id, order.Status.ToString());

            await args.CompleteMessageAsync(args.Message);
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "Erro no processamento do Service Bus.");
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_processor != null)
                await _processor.StopProcessingAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.CompletedTask;
    }
}
