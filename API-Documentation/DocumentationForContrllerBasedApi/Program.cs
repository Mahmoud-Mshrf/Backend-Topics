using DocumentationForContrllerBasedApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDependencies(builder.Configuration);
var app = builder.Build();
// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "Hello World!");
app.Run();
