using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using storageapi.Services;
using System;

namespace storageapi.Extensions
{
    public class ServiceBusOptions
    {
        public string ConnectionString { get; set; }

        // Retry policy configuration
        public string RetryMode { get; set; } ="Exponential";
        public int MaxRetries { get; set; } = 5;
        public TimeSpan Delay { get; set; } = TimeSpan.FromSeconds(0.8);
        public TimeSpan MaxDelay { get; set; } = TimeSpan.FromSeconds(8);
    }

    public static class ServiceBusClientExtensions
    {
        public static IServiceCollection AddServiceBusClientByConnectionString(this IServiceCollection services, Action<ServiceBusOptions> configureOptions)
        {
            services.Configure(configureOptions);

            services.AddAzureClients((builder) =>
            {
                var sp = services.BuildServiceProvider();
                var options = sp.GetRequiredService<IOptions<ServiceBusOptions>>().Value;

                if (string.IsNullOrWhiteSpace(options.ConnectionString))
                {
                    throw new InvalidOperationException("ServiceBus connection string is not configured.");
                }

                builder.AddServiceBusClient(options.ConnectionString)
                    .ConfigureOptions(clientOptions =>
                    {
                        clientOptions.RetryOptions = new Azure.Messaging.ServiceBus.ServiceBusRetryOptions
                        {
                            Mode = options.RetryMode?.Equals("Exponential", StringComparison.OrdinalIgnoreCase) == true
                                ? Azure.Messaging.ServiceBus.ServiceBusRetryMode.Exponential
                                : Azure.Messaging.ServiceBus.ServiceBusRetryMode.Fixed,
                            MaxRetries = options.MaxRetries,
                            Delay = options.Delay,
                            MaxDelay = options.MaxDelay
                        };
                    });
            });

            services.AddTransient<IAzureServiceBusHelper, AzureServiceBusHelper>();

            return services;
        }
    }
}
