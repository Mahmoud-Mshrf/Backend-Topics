var builder = WebApplication.CreateBuilder(args);

// in memory configuration
var inMemorySettings = new Dictionary<string, string>
{
    {"InMemoryKey", "InMemoryValue"}
};
builder.Configuration.AddInMemoryCollection(inMemorySettings);


var app = builder.Build();

app.MapGet("/{key}", (string key,IConfiguration configuration) =>
{
    return configuration[key] ?? "Key not found"; 
} );

app.Run();
