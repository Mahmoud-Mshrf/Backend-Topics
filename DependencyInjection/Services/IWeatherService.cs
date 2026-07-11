namespace DependencyInjection.Services
{
    public interface IWeatherService
    {
        Task<WeatherInfo> GetWeatherInfoAsync(string location);
    }
    public class WeatherInfo
    {
        public string Location { get; set; }
        public double Temperature { get; set; }
        public string Condition { get; set; }
    }
    public class WeatherService : IWeatherService
    {
        public async Task<WeatherInfo> GetWeatherInfoAsync(string location)
        {
            // Simulate an API call to get weather information
            await Task.Delay(500); // Simulate network delay

            // For demonstration purposes, return dummy data
            return new WeatherInfo
            {
                Location = location,
                Temperature = 25.0,
                Condition = "Sunny"
            };
        }
    }
}