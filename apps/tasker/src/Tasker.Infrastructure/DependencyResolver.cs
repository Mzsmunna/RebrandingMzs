using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Enums;
using Tasker.Application.Features.Issues;
using Tasker.Application.Features.Users;
using Tasker.Infrastructure.DB.MongoDB.Configs;
using Tasker.Infrastructure.DB.MongoDB.Repos;

namespace Tasker.Infrastructure
{
    public static class DependencyResolver
    {
        public static IServiceCollection AddTaskerInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            AddTaskerDB(services, config);
            AddTaskerEntities(services);
            AddTaskerRepositories(services);
            return services;
        }

        private static IServiceCollection AddTaskerDB(this IServiceCollection services, IConfiguration config)
        {
            services.AddMongoDB(config);
            return services;
        }

        private static IServiceCollection AddTaskerEntities(this IServiceCollection services)
        {
            services.AddScoped<UserEntityConfig>();
            //services.AddScoped<IssueEntityConfig>();
            services.AddScoped<IssueEntityConfig>(sp =>
            {
                return new IssueEntityConfig(TaskerEntities.Issue.ToString());
            });
            return services;
        }

        private static IServiceCollection AddTaskerRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IIssueRepository, IssueRepository>();   
            return services;
        }
    }
}
