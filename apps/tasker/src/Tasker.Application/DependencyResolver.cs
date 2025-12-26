using FluentValidation;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mzstruct.Common.Dependencies;
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
            services.AddValidationResolver();
            services.AddExceptionResolver();
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
