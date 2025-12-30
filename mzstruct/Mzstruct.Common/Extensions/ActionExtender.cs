using Mzstruct.Base.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Common.Extensions
{
    public static class ActionExtender
    {
        public static Action<TOptions> ToConfigureAction<TOptions>(
            this IConfigurationSection section)
            where TOptions : class, new()
        {
            return options => section.Bind(options);
        }
    }
}
