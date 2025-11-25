using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RebrandingMzs.AppHost
{
    internal static class ResourceBuilderExtensions
    {
        internal static IResourceBuilder<T> WithSwaggerUI<T>(this IResourceBuilder<T> builder)
            where T : IResourceWithEndpoints
        {
            return builder.WithOpenApiDocs("swagger-ui-docs", "Swagger API Documentation", "swagger");
        }

        internal static IResourceBuilder<T> WithScalar<T>(this IResourceBuilder<T> builder)
            where T : IResourceWithEndpoints
            {
                return builder.WithOpenApiDocs("scalar-docs", "Scalar API Documentation", "scalar/v1");
            }

        internal static IResourceBuilder<T> WithReDoc<T>(this IResourceBuilder<T> builder)
            where T : IResourceWithEndpoints
            {
                return builder.WithOpenApiDocs("redoc-docs", "ReDoc API Documentation", "api-docs");
            }

        private static IResourceBuilder<T> WithOpenApiDocs<T>(this IResourceBuilder<T> builder,
            string name,
            string displayName,
            string openApiUiPath)
            where T : IResourceWithEndpoints
        {
            return builder.WithAnnotation(new ResourceCommandAnnotation(
                name: name,
                displayName: displayName,
                displayDescription: $"Open the {displayName} API documentation in a web browser.",
                parameter: null,
                confirmationMessage: "",
                isHighlighted: true,
                executeCommand: async context =>
                {
                    try
                    {
                        // Base URL
                        var endpoint = builder.GetEndpoint("https");
                        var url = $"{endpoint. Url}/{openApiUiPath}";
                        Process. Start(new ProcessStartInfo(url) { UseShellExecute = true });
                        return new ExecuteCommandResult { Success = true };
                    }
                    catch (Exception e)
                    {
                        return new ExecuteCommandResult { Success = false, ErrorMessage = e. ToString() };
                    }
                },
                updateState: context =>
                    context.ResourceSnapshot.HealthStatus == HealthStatus.Healthy
                        ? ResourceCommandState.Enabled
                        : ResourceCommandState.Disabled,
                iconName: "Document",
                iconVariant: IconVariant.Regular
            ));
        }
    }
}
