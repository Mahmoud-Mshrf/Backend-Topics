using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore;
using OrderServiceApi.Data;
using OrderServiceApi.Repositories;
using OrderServiceApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddDbContext<AppDbContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<IOrderService, OrderService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["PaymentService:BaseUrl"]!);
});

var app = builder.Build();

app.MapControllers();

app.Run();