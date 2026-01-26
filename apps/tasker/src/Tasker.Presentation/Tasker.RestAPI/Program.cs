using Mzstruct.API.Dependencies;
using Mzstruct.Auth.Dependencies;
using Mzstruct.Base.Enums;
using Scalar.AspNetCore;
using Tasker.Application;
using Tasker.Infrastructure;
using Tasker.Infrastructure.DB.EFCore.Context;
using Tasker.Infrastructure.DB.EFCore.Helpers;

namespace Tasker.RestAPI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();
        
        // Add services to the container.
        IConfiguration _config = builder.Configuration;
        builder.Services
            //.AddAzureKV(builder.Configuration)
            .AddTaskerInfrastructure(_config)
            .AddTaskerFeatures(_config);
        //builder.Services.AddDefaultApiVersioning();
        builder.Services.AddRestApi();
        builder.Services
            .AddJwtAuth(_config, options =>
            {
                options
                    //.WithSecretKey("JWTAuthSecretKey")
                    .WithTokenExpiry(30, TimeUnit.Minutes)
                    .WithRefreshTokenExpiry(7, TimeUnit.Days);
            });

        builder.Host.UseDefaultServiceProvider(Options => 
        {
            Options.ValidateScopes = true;
            Options.ValidateOnBuild = true;
        });

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            await TaskerEFCoreHelper.SeedUsers<TaskerEFContext>(app.Services);
            await TaskerEFCoreHelper.SeedUserRoles<TaskerEFContext>(app.Services);
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

        app.UseStatusCodePages();

        app.UseDefaultFiles();

        app.UseStaticFiles();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        //app.UseOutputCache();

        app.MapControllers();

        app.Run();
    }
}
