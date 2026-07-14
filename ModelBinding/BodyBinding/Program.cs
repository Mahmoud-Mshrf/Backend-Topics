using BodyBinding.Requests;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/product", (Product product) => product.ToString());

app.Run();
