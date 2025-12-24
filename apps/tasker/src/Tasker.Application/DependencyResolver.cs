using FluentValidation;
using Mzstruct.Common.Exceptions;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Features.Auth;


namespace Tasker.Application
{
    public static class DependencyResolver
    {
        public static IServiceCollection AddTaskerFeatures(this IServiceCollection services)
        {
            AddTaskerCommands(services);
            AddTaskerQueries(services);
            AddTaskerValidators(services);
            AddTaskerExceptions(services);
            AddTaskerRepositories(services);
            AddTaskerServices(services);
            return services;
        }

        private static IServiceCollection AddTaskerCommands(IServiceCollection services)
        {
            services.AddScoped<IAuthCommand, AuthCommand>();
            return services;
        }

        private static IServiceCollection AddTaskerQueries(IServiceCollection services)
        {
            return services;
        }

        private static IServiceCollection AddTaskerValidators(IServiceCollection services)
        {
            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies(), includeInternalTypes: true);
            return services;
        }

        private static IServiceCollection AddTaskerExceptions(IServiceCollection services)
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
            return services;
        }

        private static IServiceCollection AddTaskerRepositories(IServiceCollection services)
        {
            return services;
        }

        private static IServiceCollection AddTaskerServices(IServiceCollection services)
        {
            return services;
        }
    }
}
