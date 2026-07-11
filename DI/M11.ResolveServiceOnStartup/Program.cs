var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<DbInitializer>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;

    var initializer = sp.GetRequiredService<DbInitializer>();

    initializer.Initialize();
}

app.Run();

public class DbInitializer
{
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(ILogger<DbInitializer> logger)
    {
        _logger = logger;
    }

    public void Initialize()
    {
        // logic for seeding 1000 items
        _logger.LogInformation("1000 items were seeded successfully");
    }
}