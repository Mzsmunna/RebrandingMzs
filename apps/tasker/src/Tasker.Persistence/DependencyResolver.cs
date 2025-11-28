using Kernel.Drivers.Interfaces;
using Kernel.Managers.Extensions;
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
        public static IServiceCollection AddTaskerPersistence(this IServiceCollection services, IConfiguration config)
        {
            services.AddMongoDB(config);
            services.AddScoped<UserEntityConfig>();
            services.AddScoped<IssueEntityConfig>();
            return services;
        }
    }
}
