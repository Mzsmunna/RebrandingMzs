using Kernel.Managers.Exceptions;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;


namespace Tasker.Application
{
    public static class DependencyResolver
    {
        public static IServiceCollection AddTaskerApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddTaskerExceptions(services, configuration);
            AddAppRepositories(services);
            AddTaskerAppServices(services);
            return services;
        }

        private static void AddTaskerExceptions(IServiceCollection services, IConfiguration configuration)
        {
            services.AddProblemDetails(config =>
            {
                config.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                    var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                    context.ProblemDetails.Extensions.TryAdd("traceId", activity?.TraceId.ToString() ?? string.Empty);
                };
            });
            services.AddExceptionHandler<GlobalExceptionHandler>();
        }

        private static void AddAppRepositories(IServiceCollection services)
        {
            
        }

        private static void AddTaskerAppServices(IServiceCollection services)
        {

        }
    }
}
