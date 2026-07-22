using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation_MinimalApi.Filters;
using FluentValidation_MinimalApi.Requests;
using FluentValidation_MinimalApi.Validators;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();
var app = builder.Build();

app.MapPost("/api/products", (CreateProductRequest  request) =>
{
    return Results.Created($"/api/products/{Guid.NewGuid()}",request);
}).AddEndpointFilter<ValidationFilter<CreateProductRequest>>();

app.Run();
