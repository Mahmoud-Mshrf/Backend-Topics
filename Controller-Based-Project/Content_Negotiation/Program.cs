using Content_Negotiation.Data;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddControllers();
builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable=true;//hover on it to see what it does
}).AddXmlSerializerFormatters();// add xml serialization support
builder.Services.AddSingleton<ProductRepository>();
var app = builder.Build();
app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();
