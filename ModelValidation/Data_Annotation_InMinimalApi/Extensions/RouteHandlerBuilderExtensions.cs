using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Data_Annotation_InMinimalApi.Requests;
using Microsoft.AspNetCore.Mvc;

public static class RouteHandlerBuilderExtensions
{
    // Extension method that adds validation to a Minimal API endpoint.
    public static RouteHandlerBuilder Validate<T>(this RouteHandlerBuilder builder)
    {
        // Add an endpoint filter that runs before the endpoint handler.
        builder.AddEndpointFilter(async (context, next) =>
        {
            // Get the request object of type T from the endpoint arguments.
            var argument = context.Arguments
                                  .OfType<T>()
                                  .FirstOrDefault();

            // If no request object was found, return 400 Bad Request.
            if (argument == null)
            {
                return Results.Problem(new ProblemDetails
                {
                    // Short title for the error.
                    Title = "Bad Request",

                    // Detailed error message.
                    Detail = $"{typeof(T).Name} is null",

                    // HTTP status code.
                    Status = StatusCodes.Status400BadRequest
                });
            }

            // List to store validation errors.
            List<ValidationResult> validationResults = [];

            // Validate the object using Data Annotations.
            var isValid = Validator.TryValidateObject(
                argument,
                new ValidationContext(argument),
                validationResults,
                validateAllProperties: true);

            // If validation failed...
            if (!isValid)
            {
                // Convert validation errors into the format expected by ValidationProblem().
                var errorGroups = validationResults
                    // Create one item for each property that has an error.
                    .SelectMany(v => (v.MemberNames.Any() ? v.MemberNames : new[] { "" })
                        .Select(name => new
                        {
                            name,
                            v.ErrorMessage
                        }))
                    // Group errors by property name.
                    .GroupBy(x => x.name)
                    // Convert groups into Dictionary<string, string[]>.
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage!).ToArray()
                    );

                // Return a 400 ValidationProblem response.
                return Results.ValidationProblem(errorGroups);
            }

            // Validation passed, continue to the endpoint handler.
            return await next(context);
        });

        // Return the builder to allow method chaining.
        return builder;
    }
}