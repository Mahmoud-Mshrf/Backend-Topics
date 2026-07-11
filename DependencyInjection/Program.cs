using DependencyInjection.Services;

var builder = WebApplication.CreateBuilder(args);
// how to register a service in the DI container

// builder.Services.AddSingleton<IWeatherService, WeatherService>();
// or using descriptor
builder.Services.Add(new ServiceDescriptor(
        typeof(IWeatherService),
        typeof(WeatherService),
        ServiceLifetime.Singleton));
var app = builder.Build();

app.MapGet("/{location}", async (string location, IWeatherService weatherService) =>
{
    var weatherInfo = await weatherService.GetWeatherInfoAsync(location);
    return weatherInfo;
});

app.Run();
