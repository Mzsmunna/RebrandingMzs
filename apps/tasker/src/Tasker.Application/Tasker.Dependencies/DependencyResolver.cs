using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Domain.Interfaces;
using Tasker.Infrastructure.Repositories;
using Tasker.Persistence.DAL.MongoDB;
using Tasker.Persistence.DAL.MongoDB.Configs;

namespace Tasker.Application.Dependencies
{
    public static class DependencyResolver
    {
        public static Action<TOptions> ToConfigureAction<TOptions>(
            this IConfigurationSection section)
            where TOptions : class, new()
        {
            return options => section.Bind(options);
        }

        public static void AddTaskerAppDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            AddTaskerAppConfigs(serviceCollection, configuration);
            AddAppRepositories(serviceCollection);
            AddTaskerAppServices(serviceCollection);
        }

        private static void AddTaskerAppConfigs(IServiceCollection services, IConfiguration configuration)
        {
            //builder.Services.Configure<MongoDBConfig>(_configuration.GetSection(nameof(MongoDBConfig)));
            services.Configure(configuration.GetSection(nameof(MongoDBConfig)).ToConfigureAction<MongoDBConfig>());
            services.AddScoped<MongoDBConfig>(sp => sp.GetRequiredService<IOptions<MongoDBConfig>>().Value);
            services.AddScoped<IMongoDBContext, MongoDBContext>();
            services.AddScoped<UserEntityConfig>();
            services.AddScoped<IssueEntityConfig>();
        }

        private static void AddAppRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IIssueRepository, IssueRepository>();
        }

        private static void AddTaskerAppServices(IServiceCollection services)
        {

        }
    }
}
