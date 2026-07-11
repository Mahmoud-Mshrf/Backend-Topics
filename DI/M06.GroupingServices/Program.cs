var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWeatherServices();

var app = builder.Build();
 
app.MapGet("/weather/city/{cityName}", 
(string cityName, IWeatherService weatherService) => {
 
    string? weatherInfo = weatherService.GetWeatherInfo(cityName);
    return Results.Ok(weatherInfo);
});

app.Run();

public interface IWeatherService
{
    string GetWeatherInfo(string cityName);
}

public class WeatherService : IWeatherService
{
    private IWeatherClient _weatherClient;
    public WeatherService(IWeatherClient weatherClient)
    {
        _weatherClient = weatherClient;
    }
    public string GetWeatherInfo(string cityName)
    {
        return _weatherClient.GetWeatherInfo(cityName);
    }
} 

public interface IWeatherClient
{
    string GetWeatherInfo(string cityName);
}

public class WeatherClient : IWeatherClient
{
    public string GetWeatherInfo(string cityName)
    {
        return $"Weather for {cityName} is " +
        $"{Random.Shared.Next(-10, 40)} C.";
    }
}

