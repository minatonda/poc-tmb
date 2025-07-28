// Controllers/OrderNotificationController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OrderApi.Hubs;
using OrderApi.Services;
using System.Threading.Tasks;

[ApiController]
[Route("api/notifications")]
public class OrderNotificationController : ControllerBase
{
    private readonly IHubContext<OrderHub> _hubContext;
    private readonly OrderService _service;

    public OrderNotificationController(IHubContext<OrderHub> hubContext, OrderService service)
    {
        _service = service;
        _hubContext = hubContext;
    }

    [HttpPost("order-status-changed")]
    public async Task<IActionResult> OrderStatusChanged([FromBody] OrderStatusChangedDto dto)
    {
        var order = await _service.GetByIdAsync(Guid.Parse(dto.OrderId));
        await _hubContext.Clients.All.SendAsync("OrderStatusChanged", order);
        return Ok();
    }
}

public class OrderStatusChangedDto
{
    public string OrderId { get; set; }
    public string NewStatus { get; set; }
}
