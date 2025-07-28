using Azure.Messaging.ServiceBus;
using OrderApi.Models;
using System.Text.Json;

namespace OrderApi.Services
{
    public class OrderMessageSender
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceBusClient _client;
        private readonly string _queueName;

        public OrderMessageSender(IConfiguration configuration)
        {
            _configuration = configuration;
            var connStr = _configuration["ServiceBus:ConnectionString"];
            _queueName = _configuration["ServiceBus:QueueName"];

            Console.WriteLine($"[DEBUG] ServiceBus ConnectionString: {connStr}");
            Console.WriteLine($"[DEBUG] ServiceBus QueueName: {_queueName}");

            _client = new ServiceBusClient(connStr);
        }

        public async Task SendOrderMessageAsync(Guid orderId)
        {
            var sender = _client.CreateSender(_queueName);
            var message = new ServiceBusMessage(JsonSerializer.Serialize(new { OrderId = orderId }));
            await sender.SendMessageAsync(message);
        }
    }
}
