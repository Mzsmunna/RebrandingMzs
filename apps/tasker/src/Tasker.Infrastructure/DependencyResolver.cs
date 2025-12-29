using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tasker.Application.Enums;
using Tasker.Application.Contracts.IRepos;
using Tasker.Infrastructure.DB.MongoDB.Repos;
using Mzstruct.Common.Dependencies;
using Tasker.Infrastructure.DB.MongoDB.Mappings;
using Mzstruct.DB.Providers.MongoDB.Mappers;
using Mzstruct.DB.Providers.MongoDB.Repos;

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
            //services.AddScoped<AppUserEntityMap>();
            //services.AddScoped<IssueEntityConfig>();
            services.AddScoped<IssueEntityMap>(sp =>
            {
                return new IssueEntityMap(TaskerEntities.Issue.ToString());
            });
            return services;
        }

        private static IServiceCollection AddTaskerRepositories(this IServiceCollection services)
        {
            services.AddScoped<IIssueRepository, IssueRepository>();   
            return services;
        }
    }
}
