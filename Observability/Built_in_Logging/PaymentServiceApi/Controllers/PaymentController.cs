using M01.OrderPaymentSystem.PaymentServiceApi.Models;
using M01.OrderPaymentSystem.PaymentServiceApi.Requests;
using M01.RepositoryPattern.Data;
using Microsoft.AspNetCore.Mvc;

namespace M01.OrderPaymentSystem.PaymentServiceApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentController(AppDbContext context,
                               IConfiguration configuration,
                               ILogger<PaymentController> logger
) : ControllerBase
{
    [HttpPost("process")]
    public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
    {
        logger.LogInformation("Payment process started for order {OrderId} with amount :{amount}",request.OrderId,request.Amount);
        try
        {
            if (request == null || request.OrderId == Guid.Empty || request.Amount <= 0)
            {
                logger.LogWarning("Invalid payment request for order:{orderId}, amount:{amount}",request?.OrderId,request?.Amount);
                return BadRequest("Invalid payment request.");
            }

            if (string.IsNullOrWhiteSpace(configuration["PaymentGateway:ApiKey"]))
            {
                logger.LogError("fatal: missing api key , payment failed for order:{orderId}",request.OrderId);
                throw new InvalidOperationException("Fatal: Missing API key.");
            }

            // Simulate Payment failiure chance <= 10%
            if (Random.Shared.NextDouble() <= 0.1){
                logger.LogWarning("payment process failed , order:{orderId}",request.OrderId);
                return StatusCode(502, new { Message = "Payment processing failed" });
            }

            var payment = new Payment
            {
                OrderId = request.OrderId,
                Amount = request.Amount,
                PaymentReference = $"txn_{Guid.NewGuid():N}"[..8],
                ProcessedAt = DateTime.UtcNow
            };
            logger.LogInformation("Payment Succeeded , transactionId : {transactionId},OrderId:{orderid}",payment.PaymentReference,payment.OrderId);
            await context.SaveChangesAsync();

            return Created($"/payment/{payment.PaymentReference}", new
            {
                TransactionId = payment.PaymentReference,
                Success = true
            });
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex,"unhandled exceptions during process payment for order:{orderid}",request.OrderId);
            return StatusCode(500, new { Message = "Critical error occurred." });
        }
    }
}