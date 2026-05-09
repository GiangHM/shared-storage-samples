using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using System;

namespace storageapi.Extensions
{
    public static class AppConfigurationExtensions
    {
        public static IConfigurationBuilder AddAppConfigurationBasically(this ConfigurationManager configuration)
        {
            var appConfigEndpoint = configuration.GetValue<string>("appConfig:appConfigEndpoint");
            var managedIdentity = configuration.GetValue<string>("appConfig:ClientId");

            return configuration.AddAzureAppConfiguration(options =>
            {
                options.Connect(new Uri(appConfigEndpoint), new ChainedTokenCredential(
                    new VisualStudioCredential(),
                    new ManagedIdentityCredential(managedIdentity)
                ));

                //// If using App Configuration with Azure Key Vault
                options.ConfigureKeyVault(keyVaultOptions =>
                {
                    keyVaultOptions.SetSecretRefreshInterval(TimeSpan.FromHours(60));
                    keyVaultOptions.SetCredential(new ChainedTokenCredential(
                        new VisualStudioCredential(),
                        new ManagedIdentityCredential(managedIdentity)
                    ));
                });
            });
        }

        public static IConfigurationBuilder AddAppConfigurationUsingconnectionString(this ConfigurationManager configuration)
        {
            var connectionString = configuration.GetValue<string>("appConfig:ConnectionString");

            return configuration.AddAzureAppConfiguration(options =>
            {
                options.Connect(connectionString);

                // If using App Configuration with Azure Key Vault
                //options.ConfigureKeyVault(keyVaultOptions =>
                //{
                //    keyVaultOptions.SetSecretRefreshInterval(TimeSpan.FromHours(60));
                //    keyVaultOptions.SetCredential(new ChainedTokenCredential(
                //        new VisualStudioCredential(),
                //        new DefaultAzureCredential()
                //    ));
                //});
            });
        }

    }
}
