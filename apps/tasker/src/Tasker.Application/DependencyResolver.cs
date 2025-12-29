using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Common.Dependencies;
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
            services.AddCommonFeatures();
            AddTaskerCommands(services);
            AddTaskerQueries(services);
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
    }
}
