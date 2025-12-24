
using Mzstruct.Base.Enums;
using Mzstruct.Common.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Scalar.AspNetCore;
using System.Text;
using Tasker.Application;
using Tasker.Infrastructure;

namespace Tasker.RestAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();
 
        // Add services to the container.
        IConfiguration _config = builder.Configuration;

        builder.Services.AddCors();
        builder.Services.AddHttpContextAccessor();
        builder.Services
            .AddTaskerInfrastructure(_config)
            .AddTaskerFeatures();

        builder.Services.AddControllers(options =>
        {
            options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();

        }).AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        });

        // builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddEndpointsApiExplorer();

        //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

        builder.Services
            .AddJwtAuth(_config, options =>
            {
                options
                    //.WithSecretKey("JWTAuthSecretKey")
                    .WithTokenExpiry(30, TimeUnit.Minutes)
                    .WithRefreshTokenExpiry(7, TimeUnit.Days);
            })
            .AddGoogleSignIn();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
            });
            app.UseCors(options =>
                options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );
        }
        else
        {
            app.UseCors();
        }

        app.UseExceptionHandler();

        app.UseDefaultFiles();

        app.UseStaticFiles();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
