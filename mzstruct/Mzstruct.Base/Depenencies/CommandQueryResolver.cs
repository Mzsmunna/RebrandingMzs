using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Base.Contracts.ICommands;
using Mzstruct.Base.Contracts.IEvents;
using Mzstruct.Base.Contracts.IQueries;
using Mzstruct.Base.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Depenencies
{
    public static class CommandQueryResolver
    {
        public static IServiceCollection AddCommandQueryHandlers<TClass>(this IServiceCollection services) where TClass : class, new()
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
            services.AddScoped<IDispatcher, Dispatcher>();
            return services;
        }
    }
}
