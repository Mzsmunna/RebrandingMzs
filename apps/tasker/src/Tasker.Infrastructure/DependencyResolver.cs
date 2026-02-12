using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Auth.Dependencies;
using Mzstruct.Base.Entities;
using Mzstruct.DB.Dependencies;
using Mzstruct.DB.Providers.MongoDB.Mappers;
using Tasker.Application.Contracts.IRepos;
using Tasker.Application.Enums;
using Tasker.Application.Features.Users;
using Tasker.Infrastructure.DB.EFCore.Context;
using Tasker.Infrastructure.DB.MongoDB.Mappings;
using Tasker.Infrastructure.DB.MongoDB.Repos;

namespace Tasker.Infrastructure
{
    public static class DependencyResolver
    {
        public static IServiceCollection AddTaskerInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            AddTaskerDB(services, config);
            AddTaskerMongoEntities(services);
            AddTaskerRepositories(services);
            services.AddAuth<BaseUser>(config);
            return services;
        }

        private static IServiceCollection AddTaskerDB(this IServiceCollection services, IConfiguration config)
        {
            services.AddMongoDB<MongoEntityMap>(config);
            services.AddAppDBContext<TaskerEFContext>(config);
            //services.AddIdentityDBContext<TaskerIdentityContext, TaskerUser>(config);
            return services;
        }

        private static IServiceCollection AddTaskerMongoEntities(this IServiceCollection services)
        {
            //services.AddScoped<AppUserEntityMap>();
            //services.AddScoped<IssueEntityConfig>();
            //services.AddScoped<IssueEntityMap>(sp =>
            //{
            //    return new IssueEntityMap(TaskerEntities.Issue.ToString());
            //});
            return services;
        }

        private static IServiceCollection AddTaskerRepositories(this IServiceCollection services)
        {
            services.AddScoped<IIssueRepository, IssueRepository>();
            return services;
        }
    }
}
