using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace storageapi.Extensions
{
    // This class use OpenTelemetry Distro.
    // Use Azure.Monitor.OpenTelemetry.AspNetCore package to configure Azure Monitor
    // Example configuration: builder.Services.AddOpenTelemetry().UseAzureMonitor()
    // Setting role name and role instance from configuration
    // Configure sampling rate 0.7
    public static class OpenTelemetryDistroUserDefinedExtensions
    {
        public static IServiceCollection AddUserDefinedOpenTelemetry(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var roleName = configuration["OpenTelemetry:RoleName"] ?? "storageapi";
            var roleInstance = configuration["OpenTelemetry:RoleInstance"] ?? Environment.MachineName;

            var resourceAttributes = new Dictionary<string, object> {
                { "service.name", roleName},
                { "service.instance.id", roleInstance }};

            // Configure the OpenTelemetry tracer provider to add a new processor named ActivityFilteringProcessor.
            services.ConfigureOpenTelemetryTracerProvider((sp, builder) => builder.AddProcessor(new ActivityFilteringProcessor()));

            services.AddOpenTelemetry()
                .UseAzureMonitor(option =>
                {
                    option.ConnectionString = configuration.GetValue<string>("AzureMonitor:ConnectionString");
                    option.SamplingRatio = 0.7f; // Set sampling rate to 70%
                })
                .ConfigureResource(resource =>
                {
                    resource.AddAttributes(resourceAttributes);
                });

            return services;
        }
    }

    // Filter to exclude specific requests from telemetry
    public class ActivityFilteringProcessor : BaseProcessor<Activity>
    {
        // The OnStart method is called when an activity is started. This is the ideal place to filter activities.
        public override void OnStart(Activity activity)
        {
            // prevents all exporters from exporting internal activities
            if (activity.Kind == ActivityKind.Internal)
            {
                activity.IsAllDataRequested = false;
            }
        }
    }
}
