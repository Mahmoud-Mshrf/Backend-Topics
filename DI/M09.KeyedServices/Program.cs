var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKeyedTransient<IDependency, DependencyV1>("v1");
builder.Services.AddKeyedTransient<IDependency, DependencyV2>("v2");

var app = builder.Build();

app.MapGet("/v1",([FromKeyedServices("v1")] IDependency dependency) =>{
    var response = dependency.DoSomething();
    return Results.Ok(response);
});

app.MapGet("/v2",([FromKeyedServices("v2")] IDependency dependency) =>{
    var response = dependency.DoSomething();
    return Results.Ok(response);
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