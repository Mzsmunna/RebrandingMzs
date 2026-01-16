using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mzstruct.Base.Enums;
using Mzstruct.DB.EFCore.Context;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Features.Users;
using Tasker.Infrastructure.DB.EFCore.Context;

namespace Tasker.Infrastructure.DB.EFCore.Helpers
{
    public static class TaskerEFCoreHelper
    {
        public static async Task SeedData<TContext>(IServiceProvider services) where TContext : DbContext
        {
            await using(var serviceScope = services.CreateAsyncScope())
            //await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<TaskerEFContext>())
            await using (var dbContext = serviceScope.ServiceProvider.GetService<TaskerEFContext>())
            {
                if (dbContext == null) return;
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
        }
    }
}
