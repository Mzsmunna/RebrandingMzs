using FluentValidation;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mzstruct.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Contracts.ICommands;
using Tasker.Application.Contracts.IQueries;
using Tasker.Application.Features.Auth;
using Tasker.Application.Features.Issues;
using Tasker.Application.Features.Users;

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
            //AddTaskerRepositories(services);
            //AddTaskerServices(services);
            return services;
        }

        private static IServiceCollection AddTaskerCommands(IServiceCollection services)
        {
            services.AddScoped<IAuthCommand, AuthCommand>();
            services.AddScoped<IUserCommand, UserCommand>();
            services.AddScoped<IIssueCommand, IssueCommand>();
            return services;
        }

        private static IServiceCollection AddTaskerQueries(IServiceCollection services)
        {
            services.AddScoped<IUserQuery, UserQuery>();
            services.AddScoped<IIssueQuery, IssueQuery>();
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
