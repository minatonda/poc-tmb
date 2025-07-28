using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Models;

namespace OrderApi.Services
{
    public class OrderService
    {
        private readonly OrderDbContext _context;
        private readonly IConfiguration _configuration;

        public OrderService(OrderDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Order>> GetAllAsync() =>
            await _context.Orders.ToListAsync();

        public async Task<Order?> GetByIdAsync(Guid id) =>
            await _context.Orders.FindAsync(id);

        public async Task<Order> CreateAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var sender = new OrderMessageSender(_configuration);
            await sender.SendOrderMessageAsync(order.Id);
            return order;
        }

        public async Task UpdateStatusAsync(Guid id, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
            }
        }
    }
}
