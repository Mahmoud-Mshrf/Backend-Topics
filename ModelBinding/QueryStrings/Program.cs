using QueryStrings.classes;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
// query string from object
app.MapGet("/minimalreq", ([AsParameters] SearchRequest request) => request);// in case of Controller api we use [fromQuery] instead of [AsParameters]
// from array
app.MapGet("/minimalreq-array", (int[] nums) => nums);

app.Run();
