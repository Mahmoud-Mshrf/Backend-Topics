using Microsoft.AspNetCore.Mvc;
using Structured_Logging.Services;

namespace Structured_Logging.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(OrderService orderService) : ControllerBase
{
    [HttpPost("{orderId:guid}")]
    public async Task<IActionResult> ProcessOrder(System.Guid orderId)
    {
        Guid userId = Guid.Parse("e4d72cd3-f0ec-4bfc-f7e1-08deec75c768");

        await orderService.ProcessOrder(Guid.NewGuid(),userId);
        
        return Ok(new
        {
            OrderId = orderId,
            Status = "Processed"
        });
    }
}