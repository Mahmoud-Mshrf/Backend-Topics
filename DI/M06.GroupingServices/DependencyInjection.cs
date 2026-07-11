namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
    public static IServiceCollection AddWeatherServices(this IServiceCollection services)
    {
        services.AddTransient<IWeatherClient, WeatherClient>();

        services.AddTransient<IWeatherService, WeatherService>();

        return services;
    }
}