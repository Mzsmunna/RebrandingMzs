using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Managers.Extensions
{
    public static class SharedExtender
    {
        public static Action<TOptions> ToConfigureAction<TOptions>(
            this IConfigurationSection section)
            where TOptions : class, new()
        {
            return options => section.Bind(options);
        }
    }
}
