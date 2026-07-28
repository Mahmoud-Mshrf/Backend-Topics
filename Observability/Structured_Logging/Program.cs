using Structured_Logging.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<OrderService>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();
app.Run();
