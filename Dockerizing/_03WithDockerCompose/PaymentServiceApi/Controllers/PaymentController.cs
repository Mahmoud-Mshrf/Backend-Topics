using Microsoft.AspNetCore.Mvc;
using PaymentServiceApi.Data;
using PaymentServiceApi.Models;
using PaymentServiceApi.Requests;

namespace PaymentServiceApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentController(AppDbContext context) : ControllerBase
{
    [HttpPost("process")]
    public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
    {
        // Simulate processing delay
        await Task.Delay(Random.Shared.Next(100, 500));

        // Mock success/failure

        var success = Random.Shared.NextDouble() > 0.1;

        if (!success)
            return StatusCode(502, new { Message = "Payment processing failed." });


        var payment = new Payment
        {
            OrderId = request.OrderId,
            Amount = request.Amount,
            PaymentReference = $"txn_{Guid.NewGuid().ToString("N")[..8]}",
            ProcessedAt = DateTime.UtcNow
        };

        await context.SaveChangesAsync();

        return Created($"/payment/{payment.PaymentReference}", new
        {
            TransactionId = payment.PaymentReference,
            Success = true
        });
    }
}