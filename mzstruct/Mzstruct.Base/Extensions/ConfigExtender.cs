using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Mzstruct.Base.Extensions
{
    public static class ConfigExtender
    {
        public static IConfiguration? Config
        {
            get
            {
                return (Config is null) ? GetNewConfig() : Config;
            }
            set
            {
                if (Config is null && value != null)
                    Config = value;
                else
                    Config = GetNewConfig();
            }
        }
        public static IConfiguration GetNewConfig()
        {
            return new ConfigurationBuilder()
                .AddSettingsJson()
                .AddEnvironmentVariables()
                .Build();
        }

        public static string? GetAppSetting(this IConfiguration config, string key) => config.GetValue<string>(key);
        public static T? GetAppSetting<T>(this IConfiguration config, string key) => config.GetSection(key).Get<T>();
        public static IConfigurationBuilder AddSettingsJson(this IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true);
            return builder;
        }
    }
}
