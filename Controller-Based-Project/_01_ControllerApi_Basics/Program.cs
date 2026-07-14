using _01_ControllerApi_Basics.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<ProductRepository>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapControllers();
app.Run();
