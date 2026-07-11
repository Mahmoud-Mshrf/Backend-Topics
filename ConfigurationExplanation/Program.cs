using ConfigurationExplanation;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AttachmentsOptions>(builder.Configuration.GetSection("AttachmentsOptions"));
// or
builder.Services.AddOptions<AttachmentsOptions>().Bind(builder.Configuration.GetSection("AttachmentsOptions"));

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
app.MapGet("/get-by-key/{key}", (string key,IConfiguration configuration) =>
{
    return configuration.GetValue<string>(key) ?? "Key not found"; 
} );
app.MapGet("/get-connection-string/{key}", (string key,IConfiguration configuration) =>
{
    return configuration.GetConnectionString(key) ?? "Key not found";
} );
app.MapGet("/AttachmentsOptions", (IConfiguration configuration) =>
{
    // return configuration.GetSection("AttachmentsOptions").Get<AttachmentsOptions>() ?? new AttachmentsOptions();
    var attachmentsOptions = new AttachmentsOptions();
    configuration.GetSection("AttachmentsOptions").Bind(attachmentsOptions);
    return attachmentsOptions;
});
// singleton options at startup , any change in configuration will not be reflected in the options instance until the application is restarted
app.MapGet("/AttachmentsOptions-From-Options", (IOptions<AttachmentsOptions> options) =>
{
    return options.Value;
});
// singleton options at runtime , any change in configuration will be reflected in the options instance in the current request doesn't wait for the next request to reflect the changes
app.MapGet("/AttachmentsOptions-From-OptionsMonitor", (IOptionsMonitor<AttachmentsOptions> optionsMonitor) =>
{
    return optionsMonitor.CurrentValue;
});
// singleton options at runtime , any change in configuration will be reflected in the options instance starting from the next request
app.MapGet("/AttachmentsOptions-From-OptionsSnapshot", (IOptionsSnapshot<AttachmentsOptions> optionsSnapshot) =>
{
    return optionsSnapshot.Value;
});
app.Run();
