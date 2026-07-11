var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IPaymentProvider>(sp => {
    var config = sp.GetRequiredService<IConfiguration>();
    return config["PaymentProvider"] == "Stripe" ? new StripePayment() : new PayPalPayment(); 
});

var app = builder.Build();

app.MapGet("/pay/{amount}", (decimal amount, IPaymentProvider paymentProvider) => {
    var response = paymentProvider.Pay(amount);
    return Results.Ok(response);
});

app.Run();
 
public interface IPaymentProvider
{
    string Pay(decimal amount);
}

public class StripePayment : IPaymentProvider
{
    public string Pay(decimal amount)
        => $"Payment of ${amount} was processed using Strip!";
}

public class PayPalPayment : IPaymentProvider
{
    public string Pay(decimal amount)
        => $"Payment of ${amount} was processed using PayPal!";
}