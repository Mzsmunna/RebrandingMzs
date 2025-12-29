using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Common.Auth;
using Mzstruct.Common.Contracts.ICommands;
using Mzstruct.Common.Contracts.IQueries;
using Mzstruct.Common.Features.Users;
using Mzstruct.DB.Providers.MongoDB.Contracts.IRepos;
using Mzstruct.DB.Providers.MongoDB.Mappers;
using Mzstruct.DB.Providers.MongoDB.Repos;

namespace Mzstruct.Common.Dependencies
{
    public static class DependencyResolver
    {
        public static IServiceCollection AddCommonFeatures(this IServiceCollection services)
        {
            AddAppCommands(services);
            AddAppQueries(services);
            AddAppEntities(services);
            AddAppRepositories(services);
            AddAppServices(services);
            services.AddValidationResolver();
            services.AddExceptionResolver();
            return services;
        }

        private static IServiceCollection AddAppCommands(IServiceCollection services)
        {
            services.AddScoped<IAppAuthCommand, AppAuthCommand>();
            services.AddScoped<IAppUserCommand, AppUserCommand>();
            return services;
        }

        private static IServiceCollection AddAppQueries(IServiceCollection services)
        {
            services.AddScoped<IAppUserQuery, AppUserQuery>();
            return services;
        }

        private static IServiceCollection AddAppEntities(this IServiceCollection services)
        {
            services.AddScoped<AppUserEntityMap>();
            return services;
        }

        private static IServiceCollection AddAppRepositories(IServiceCollection services)
        {
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            return services;
        }

        private static IServiceCollection AddAppServices(IServiceCollection services)
        {
            return services;
        }
    }
}
