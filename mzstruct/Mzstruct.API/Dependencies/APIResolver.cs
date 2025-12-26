using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Auth.Contracts.IManagers;
using Mzstruct.Auth.Managers;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.API.Dependencies
{
    public static class APIResolver
    {
        public static IServiceCollection AddRestApi(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors();
            services.AddHttpContextAccessor();

            // services.AddControllers();
            services.AddControllers(options =>
            {
                options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();

            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
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
    }
}
