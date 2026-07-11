using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.TryAddTransient<IDependency, DependencyV1>();
builder.Services.AddTransient<IDependency, DependencyV2>();
builder.Services.TryAddTransient<IDependency, DependencyV1>();
 
var app = builder.Build();

app.MapGet("/single",(IDependency dependency) =>{
    var response = dependency.DoSomething();
    return Results.Ok(response);
});

app.MapGet("/multiple",(IEnumerable<IDependency> dependencies) => {
    var response = string.Empty;
    foreach (var dependency in dependencies)
    {
        response += dependency.DoSomething();
    }
    return Results.Ok(response);
});
 
app.MapGet("/idependency-registrations",(IServiceProvider sp) => {
  
  var servicesRegisteredCount = sp.GetServices<IDependency>();

    return Results.Ok(servicesRegisteredCount.Count());
});
 

app.Run();

public interface IDependency
{
    string DoSomething();
}

public class DependencyV1 : IDependency
{
    public string DoSomething()
    => "Something done using V1 !!";
}

public class DependencyV2 : IDependency
{
    public string DoSomething()
    => "Something done using V2 !!";
}