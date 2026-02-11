using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Depenencies
{
    public static class ExceptionResolver
    {
        public static IServiceCollection AddGlobalExceptionResolver(this IServiceCollection services)
        {
            services.AddProblemDetails(config =>
            {
                config.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path.Value}";
                    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                    var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                    context.ProblemDetails.Extensions.TryAdd("traceId", activity?.TraceId.ToString() ?? string.Empty);
                };
            });
            services.AddExceptionHandler<GlobalExceptionHandler>();
            return services;
        }
    }
}
