using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using System.Text.Json.Nodes;

namespace DocumentationForContrllerBasedApi.OpenApi.Transformers;

internal sealed class ApiVersionDefaultTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (operation.Parameters is null)
            return Task.CompletedTask;

        var versionParam = operation.Parameters
            .FirstOrDefault(p => p.Name == "api-version");

        if (versionParam is OpenApiParameter concreteParam)
        {
            var versionNumber = context.DocumentName.TrimStart('v'); // "v1" -> "1"
            var versionValue = $"{versionNumber}.0";                  // "1"  -> "1.0"

            concreteParam.Schema ??= new OpenApiSchema();

            if (concreteParam.Schema is OpenApiSchema concreteSchema)
            {
                concreteSchema.Default = JsonValue.Create(versionValue);
            }
        }

        return Task.CompletedTask;
    }
}