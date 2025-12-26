using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.API.Mappings
{
    public static class APIMapper
    {
        public static IServiceCollection MapRestApi(this IServiceCollection services)
        {
            return services;
        }
    }
}
