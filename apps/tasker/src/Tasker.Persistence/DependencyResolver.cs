using Kernel.Drivers.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Interfaces;
using Tasker.Persistence.DAL.MongoDB;
using Tasker.Persistence.DAL.MongoDB.Configs;

namespace Tasker.Persistence
{
    public static class DependencyResolver
    {
        public static Action<TOptions> ToConfigureAction<TOptions>(
            this IConfigurationSection section)
            where TOptions : class, new()
        {
            return options => section.Bind(options);
        }

        public static IServiceCollection AddTaskerPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            //builder.Services.Configure<MongoDBConfig>(_configuration.GetSection(nameof(MongoDBConfig)));
            services.Configure(configuration.GetSection(nameof(MongoDBConfig)).ToConfigureAction<MongoDBConfig>());
            services.AddScoped<MongoDBConfig>(sp => sp.GetRequiredService<IOptions<MongoDBConfig>>().Value);
            services.AddScoped<IMongoDBContext, MongoDBContext>();
            services.AddScoped<UserEntityConfig>();
            services.AddScoped<IssueEntityConfig>();
            return services;
        }
    }
}
