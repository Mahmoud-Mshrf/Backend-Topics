var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<RemoveOrphansFilesBackgroundService>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
