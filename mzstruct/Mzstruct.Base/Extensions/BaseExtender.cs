using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Extensions
{
    public static class BaseExtender
    {
        public static string? GetAppSetting(this IConfiguration config, string key) => config.GetValue<string>(key);
        public static T? GetAppSetting<T>(this IConfiguration config, string key) => config.GetSection(key).Get<T>();
    }
}
