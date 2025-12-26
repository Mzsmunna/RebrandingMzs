using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Common.Dependencies
{
    public static class ValidationResolver
    {
        public static IServiceCollection AddValidationResolver(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies(), includeInternalTypes: true);
            return services;
        }
    }
}
