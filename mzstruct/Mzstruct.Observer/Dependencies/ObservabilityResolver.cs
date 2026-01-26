using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Observer.Dependencies
{
    public static class ObservabilityResolver
    {
        public static IServiceCollection AddOpenTelemetryObserver(this IServiceCollection services, ILoggingBuilder logging, string resourceName)
        {
            services.AddOpenTelemetry()
                .ConfigureResource(rsc => rsc.AddService(resourceName))
                .WithMetrics(mtrc =>
                {
                    //mtrc.AddMeter( // Manual approach
                    //    "Microsoft.AspNetCore.Hosting",
                    //    "Microsoft.AspNetCore.Server.Kestrel",
                    //    "System. Net.Http"
                    //);

                    mtrc.AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation();

                    //mtrc.AddMeter(DiagnosticsConfig.Meter.Name);
                    //mtrc.AddConsoleExporter();
                    mtrc.AddOtlpExporter();
                })
                .WithTracing(trc =>
                {
                    trc.AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddEntityFrameworkCoreInstrumentation();
                    //trc.AddConsoleExporter();
                    trc.AddOtlpExporter();
                });

            logging.AddOpenTelemetry(log => log.AddOtlpExporter());
            return services;
        }
    }
}
