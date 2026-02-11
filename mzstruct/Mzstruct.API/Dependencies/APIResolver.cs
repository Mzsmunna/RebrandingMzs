using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.RateLimiting;

namespace Mzstruct.API.Dependencies
{
    public static class APIResolver
    {
        public static IServiceCollection AddRestApi(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors();
            services.AddHttpClient();
            services.AddHttpContextAccessor();

            // services.AddControllers();
            services.AddControllers(options =>
            {
                options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();

            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();
            services.AddEndpointsApiExplorer();

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            //            .GetBytes(_configuration.GetValue<string>("JWTAuthSecretKey")!)),
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //        ValidateLifetime = true,
            //        ClockSkew = TimeSpan.Zero
            //    };
            //});

            return services;
        }

        public static IServiceCollection AddDefaultApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                //options.ApiVersionReader = ApiVersionReader.Combine(
                //    new UrlSegmentApiVersionReader(),
                //    new HeaderApiVersionReader("x-api-version"),
                //    new QueryStringApiVersionReader("api-version"),
                //    new MediaTypeApiVersionReader()
                //);
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                // to work with swagger
                options.GroupNameFormat = "v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddFeatureManagement();
            return services;
        }

        public static IServiceCollection AddApiRateLimiter(this IServiceCollection services, IConfiguration config)
        {
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("fixed", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 100;
                    limiterOptions.Window = TimeSpan.FromMinutes(1);
                    limiterOptions.QueueLimit = 0;
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });

                options.AddTokenBucketLimiter("token", limiterOptions =>
                {
                    limiterOptions.TokenLimit = 50;
                    limiterOptions.TokensPerPeriod = 25;
                    limiterOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(30);
                    limiterOptions.AutoReplenishment = true;
                    limiterOptions.QueueLimit = 0;
                });

                options.AddPolicy("ip-policy", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 60,
                            Window = TimeSpan.FromMinutes(1)
                        })
                );

                options.AddSlidingWindowLimiter("sliding", o =>
                {
                    o.PermitLimit = 100;
                    o.Window = TimeSpan.FromMinutes(1);
                    o.SegmentsPerWindow = 4;
                    o.QueueLimit = 0;
                });

                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });
            return services;
        }
    }
}
