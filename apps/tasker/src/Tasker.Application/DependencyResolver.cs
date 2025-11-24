using Microsoft.AspNetCore.Builder.Extensions;
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
            services.AddProblemDetails(conf =>
            {
                conf.CustomizeProblemDetails = cntxt =>
                {
                    cntxt.ProblemDetails.Extensions.TryAdd("requestId", cntxt.HttpContext.TraceIdentifier);
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
