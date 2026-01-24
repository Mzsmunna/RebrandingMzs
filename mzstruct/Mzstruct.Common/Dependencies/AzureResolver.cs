using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Auth.Contracts.IManagers;
using Mzstruct.Auth.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Common.Dependencies
{
    public static class AzureResolver
    {
        public static IServiceCollection AddAzureKV(this IServiceCollection services, ConfigurationManager configure, bool useSecretClient = false)
        {
            // Key Vault URL from config or env: "https://my-app-kv.vault.azure.net/"
            var keyVaultUrl = configure["KeyVault:Url"];
            if (string.IsNullOrWhiteSpace(keyVaultUrl))
                throw new ArgumentNullException("KeyVault:Url", "Key Vault URL is not configured.");

            if (useSecretClient is false)
            {
                // DefaultAzureCredential works locally (az login / VS) and in Azure (Managed Identity)
                configure.AddAzureKeyVault(
                    new Uri(keyVaultUrl),
                    new DefaultAzureCredential());
            }
            else
            {
                var credential = new DefaultAzureCredential();
                var client = new SecretClient(new Uri(keyVaultUrl), credential);
                var options = new AzureKeyVaultConfigurationOptions
                {
                    ReloadInterval = TimeSpan.FromMinutes(5) // optional
                };
                configure.AddAzureKeyVault(client, options); // or with options
            }
            return services;
        }
    }
}
