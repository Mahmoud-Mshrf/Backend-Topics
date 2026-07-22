using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});// make enum values translated to string not int in serialization

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();
app.Run();
