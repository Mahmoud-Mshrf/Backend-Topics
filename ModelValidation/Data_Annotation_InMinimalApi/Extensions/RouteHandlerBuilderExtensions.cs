using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Data_Annotation_InMinimalApi.Requests;
using Microsoft.AspNetCore.Mvc;

public static class RouteHandlerBuilderExtensions
{
    public static  RouteHandlerBuilder Validate<T>(this RouteHandlerBuilder builder)
    {
        builder.AddEndpointFilter(async(context, next) =>
        {
            var argument = context.Arguments.OfType<CreateProductRequest>().FirstOrDefault();

            if (argument == null)
            {
                return Results.Problem(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = $"{nameof(CreateProductRequest)} is null",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            List<ValidationResult> validationResults = [];

            var isValid = Validator.TryValidateObject(argument,new ValidationContext(argument),validationResults,true);

            if (!isValid)
            {
                var errorGroups = validationResults.SelectMany(v=> (v.MemberNames.Any() ?  v.MemberNames : new [] {""})
                                                   .Select(name=> new {name , v.ErrorMessage})
                                                   .GroupBy(x=>x.name)
                                                   .ToDictionary(
                                                      g=>g.Key,
                                                      g=>g.Select(x=>x.ErrorMessage!).ToArray()
                                                   ));
                return Results.ValidationProblem(errorGroups);
            }
            return await next(context);
        });
        return builder;
    }
}