using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Base.Consts;
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
        public static IServiceCollection AddCommonFeatures(this IServiceCollection services, IConfiguration config)
        {
            AppConst.Init(config, services);
            AddAppCommands(services);
            AddAppQueries(services);
            AddAppServices(services);
            services.AddValidationResolver();
            services.AddExceptionResolver();
            return services;
        }

        private static IServiceCollection AddAppCommands(IServiceCollection services)
        {
            services.AddScoped<IAuthCommand, AuthCommand>();
            services.AddScoped<IUserCommand, UserCommand>();
            return services;
        }

        private static IServiceCollection AddAppQueries(IServiceCollection services)
        {
            services.AddScoped<IUserQuery, UserQuery>();
            return services;
        }

        private static IServiceCollection AddAppServices(IServiceCollection services)
        {
            //services.AddKeyedScoped<IAuthService, AuthService>("jwt_auth");
            //services.AddKeyedScoped<IAuthService, IdentityAuthService>("identity_auth");
            //then inside the service dependency innject as: [FromKeyedServices("jwt_auth")] IAuthService authService,
            return services;
        }
    }
}
