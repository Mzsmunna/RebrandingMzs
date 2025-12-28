using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Enums;
using Tasker.Application.Contracts.IRepos;
using Tasker.Infrastructure.DB.MongoDB.Repos;
using Mzstruct.Common.Dependencies;
using Tasker.Infrastructure.DB.MongoDB.Mappings;

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
            services.AddScoped<UserEntityMap>();
            //services.AddScoped<IssueEntityConfig>();
            services.AddScoped<IssueEntityMap>(sp =>
            {
                return new IssueEntityMap(TaskerEntities.Issue.ToString());
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
