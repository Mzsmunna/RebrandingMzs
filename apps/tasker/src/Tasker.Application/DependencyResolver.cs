using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;


namespace Tasker.Application
{
    public static class DependencyResolver
    {
        public static IServiceCollection AddTaskerApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddTaskerAppConfigs(services, configuration);
            AddAppRepositories(services);
            AddTaskerAppServices(services);
            return services;
        }

        private static void AddTaskerAppConfigs(IServiceCollection services, IConfiguration configuration)
        {
            
        }

        private static void AddAppRepositories(IServiceCollection services)
        {
            
        }

        private static void AddTaskerAppServices(IServiceCollection services)
        {

        }
    }
}
