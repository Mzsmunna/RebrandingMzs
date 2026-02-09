using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Patterns.CQRS
{
    public static class RequestResolver
    {
        public static IServiceCollection AddCommandQueryHandlers<TClass>(this IServiceCollection services) where TClass : class
        {
            services.Scan(scan => scan
                //.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                //.AddClasses(classes => classes
                //    .InNamespaces("Mzstruct.Base.CommandHandlers", "Mzstruct.Base.QueryHandlers"))
                .FromAssemblyOf<TClass>()
                .AddClasses(classes => classes
                    .AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                .AddClasses(classes => classes
                    .AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );
            services.AddScoped<IRequestSender, RequestSender>();
            return services;
        }
    }
}
