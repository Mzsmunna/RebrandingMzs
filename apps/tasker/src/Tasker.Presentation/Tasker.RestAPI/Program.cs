using Microsoft.EntityFrameworkCore;
using Mzstruct.API.Dependencies;
using Mzstruct.Base.Enums;
using Mzstruct.Common.Dependencies;
using Scalar.AspNetCore;
using Tasker.Application;
using Tasker.Application.Features.Users;
using Tasker.Infrastructure;
using Tasker.Infrastructure.DB.EFCore.Context;

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
            .AddTaskerInfrastructure(_config)
            .AddTaskerFeatures();
        //builder.Services.AddDefaultApiVersioning();
        builder.Services.AddRestApi();
        builder.Services
            .AddJwtAuth(_config, options =>
            {
                options
                    //.WithSecretKey("JWTAuthSecretKey")
                    .WithTokenExpiry(30, TimeUnit.Minutes)
                    .WithRefreshTokenExpiry(7, TimeUnit.Days);
            })
            .AddGoogleSignIn();

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
            await using(var serviceScope = app.Services.CreateAsyncScope())
            await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<TaskerEFContext>())
            {
                dbContext.Database.Migrate();
                if (!dbContext.Users.Any())
                {
                    await dbContext.Users.AddRangeAsync([new User
                    {
                        Id = Guid.CreateVersion7().ToString(),
                        Name = "Mzs Munna",
                        Email = "mzs.munna@gmail.com",
                        Username = "mzsmunna",
                        Password = "P@ssw0rd123",
                        Role = "Admin",
                    },
                    new User
                    {
                        Id = Guid.CreateVersion7().ToString(),
                        Name = "Mamunuz Zaman",
                        Email = "mzaman@insightintechnology.com",
                        Username = "mzaman",
                        Password = "P@ssw0rd321",
                        Role = "User",
                    }]);
                    dbContext.SaveChanges();
                }
                //await dbContext.Database.EnsureCreatedAsync();
            }

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

        app.MapControllers();

        app.Run();
    }
}
