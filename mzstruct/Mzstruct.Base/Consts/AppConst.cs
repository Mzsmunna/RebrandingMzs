using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mzstruct.Base.Consts
{
    public static class AppConst
    {
        private static IConfiguration? _config { get; set; } 
        private static IServiceCollection? _services { get; set; }
        private static IServiceProvider? _serviceProvider { get; set; }

        public static void Init(IConfiguration? config,
            IServiceCollection? services)
        {
            if (_config == null && config != null) _config = config;
            if (_services == null && services != null) _services = services;
        }

        public static void Build(IServiceProvider? serviceProvider)
        {
            if (_serviceProvider == null && serviceProvider != null) _serviceProvider = serviceProvider;
        }

        public static IConfiguration? GetConfig() => _config;
        public static IServiceCollection? GetServices() => _services;
        public static IServiceProvider? GetServiceProvider() => _serviceProvider;
        public static string? GetAppSetting(string key) => _config?.GetValue<string>(key); // if (_config is not null) // && _configuration.GetChildren().Any(item => item.Key == key)
		public static T? GetAppSetting<T>(string key) => _config is null ? default : _config.GetSection(key).Get<T>();
		public static string? DatabaseContext => _config?.GetValue<string>("ConnectionStrings:DatabaseContext");
		public static string? SqlConnectionString => _config?.GetValue<string>("ConnectionStrings:DatabaseContext");
		public static string? ConnectionString => _config?.GetValue<string>("MongoDBSettings:ConnectionString");
		public static string? DatabaseName => _config?.GetValue<string>("MongoDBSettings:DatabaseName");
		public static string? AzureConnectionString => _config?.GetValue<string>("AzureConnectionString");
		public static string? AppId => _config?.GetValue<string>("AppId");
		public static string? ClientId => _config?.GetValue<string>("ClientId");
    }
}
