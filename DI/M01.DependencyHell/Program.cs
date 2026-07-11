
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/weather/city/{cityName}", (string cityName) => {

    var weatherService = new WeatherService();
    string weatherInfo = weatherService.GetWeatherInfo(cityName);
    return Results.Ok(weatherInfo);
});


app.Run();

class WeatherService
{ 
    public string GetWeatherInfo(string cityName)
    {
        var weatherClient = new WeatherClient();
        return weatherClient.GetWeatherInfo(cityName); 
    }
}

class WeatherClient
{
    public string GetWeatherInfo(string cityName)
    {
        var http = new HttpClient();
        // some external http call
        return $"Weather for {cityName} is {Random.Shared.Next(-10, 40)} C.";
    }
}