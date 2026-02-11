using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Auth.Contracts.IServices;
using Mzstruct.Auth.Services;
using Mzstruct.Base.Consts;
using Mzstruct.Base.Depenencies;
using Mzstruct.Common.Contracts.ICommands;
using Mzstruct.Common.Contracts.IQueries;
using Mzstruct.Common.Features.Users;

namespace Mzstruct.Common.Dependencies
{
    public static class CommonResolver
    {
        public static IServiceCollection AddCommonFeatures(this IServiceCollection services, IConfiguration config)
        {
            AppConst.Init(config, services);
            AddAppCommands(services);
            AddAppQueries(services);
            AddAppServices(services);
            services.AddValidationResolver();
            services.AddGlobalExceptionResolver();
            return services;
        }

        private static IServiceCollection AddAppCommands(IServiceCollection services)
        {
            services.AddScoped<IUserCommandService, UserCommandService>();
            return services;
        }

        private static IServiceCollection AddAppQueries(IServiceCollection services)
        {
            services.AddScoped<IUserQueryService, UserQueryService>();
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
