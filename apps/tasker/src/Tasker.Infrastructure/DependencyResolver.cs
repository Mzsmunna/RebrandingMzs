using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Interfaces;
using Tasker.Infrastructure.DB.MongoDB.Configs;
using Tasker.Infrastructure.DB.MongoDB.Repositories;

namespace Tasker.Infrastructure
{
    public static class DependencyResolver
    {
        public static IServiceCollection AddTaskerInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddMongoDB(config);
            services.AddScoped<UserEntityConfig>();
            services.AddScoped<IssueEntityConfig>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IIssueRepository, IssueRepository>();
            return services;
        }
    }
}
