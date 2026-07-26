using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using Microsoft.VisualBasic;

namespace DocumentationForContrllerBasedApi.OpenApi.Transformers;

internal sealed class VersionInfoTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var version =context.DocumentName;
        document.Info.Version=version;
        document.Info.Title= $"Project Api {version}";

        return Task.CompletedTask;
    }
}
