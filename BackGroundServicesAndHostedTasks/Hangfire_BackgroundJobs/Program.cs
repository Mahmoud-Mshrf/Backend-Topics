using Hangfire;
using Hangfire.Storage.SQLite;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// register hangfire and use sql server as database for persistency 
// builder.Services.AddHangfire(config =>
// {
//     config.UseSQLiteStorage(builder.Configuration.GetConnectionString("Hangfire"));
// });
var databasePath = Path.Combine("Data", "HangfireDatabase.db");

Directory.CreateDirectory(Path.GetDirectoryName(databasePath)!);

builder.Services.AddHangfire(config =>
{
    config.UseSQLiteStorage(databasePath);
});
// assign background workers for hangfire
builder.Services.AddHangfireServer();
var app = builder.Build();
app.UseHangfireDashboard("/hangfire");
app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
