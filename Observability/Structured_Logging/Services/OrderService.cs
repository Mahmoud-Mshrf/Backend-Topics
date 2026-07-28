namespace Structured_Logging.Services;
public class OrderService(ILogger<OrderService> logger)
{
    public Task ProcessOrder(Guid orderId,Guid userId)
    {
        // logger.LogInformation($"Order {orderId} is processed for user : {userId}"); // UnStructured logging (Bad)

        logger.LogInformation("Order {orderId} is processed for user {userId}",orderId,userId); // Structured logging (Good)

        return Task.CompletedTask;
    }
}