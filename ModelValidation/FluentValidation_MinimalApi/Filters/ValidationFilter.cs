using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FluentValidation_MinimalApi.Filters;

public class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        var model = context.Arguments.OfType<T>().FirstOrDefault();

        if (validator == null || model == null)
        {
            return  Results.Problem(new ProblemDetails
            {
                Title = "Bad Request",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"{nameof(T)} is null"
            });
        }

        var result =await validator.ValidateAsync(model);

        if (!result.IsValid)
        {
            var errors = result.Errors.GroupBy(x=>x.PropertyName)
            .ToDictionary(x=>x.Key,x=>x.Select(x=>x.ErrorMessage!).ToArray());

            return Results.ValidationProblem(errors);
        }
        return await next(context);
    }
}