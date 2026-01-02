using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mzstruct.DB.EFCore.Configs;
using Tasker.Application.Features.Users;

namespace Tasker.Infrastructure.DB.EFCore.Configs
{
    public class UserEFConfig : BaseUserEFConfig<User> //BaseEntityEFConfig<User>
    {
        public UserEFConfig(string? tableName = "User") : base(tableName) { }

        public override void Configure(EntityTypeBuilder<User> builder)
        {
            //builder.HasKey(u => u.Id);
            //builder.Property(x => x.Id).ValueGeneratedNever();
            //builder.HasKey(u => new { u.Id, u.Email, u.Username });
            //builder.HasIndex(e => new { e.Id, e.Email, e.Username }).IsUnique();

            builder.HasMany(f => f.AssignerIssues)
                .WithOne(f => f.Assigner)
                .HasForeignKey(f => f.AssignerId);

            builder.HasMany(f => f.UserIssues)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId);

            builder.Navigation(f => f.AssignerIssues).AutoInclude();
            builder.Navigation(f => f.UserIssues).AutoInclude();

            //builder.HasData([new User
            //{
            //    Id = "0ba69e11-483b-4649-8634-57ac875f03d8", //Guid.CreateVersion7().ToString(),
            //    Name = "Mzs Munna",
            //    Email = "mzs.munna@gmail.com",
            //    Username = "mzsmunna",
            //    Password = "P@ssw0rd123",
            //    Role = "Admin",
            //    //CreatedAt = DateTime.UtcNow,
            //},
            //new User
            //{
            //    Id = "be32d5c7-25d6-434e-a07e-49b2f617b1b5", //Guid.CreateVersion7().ToString(),
            //    Name = "Mamunuz Zaman",
            //    Email = "mzaman@insightintechnology.com",
            //    Username = "mzaman",
            //    Password = "P@ssw0rd321",
            //    Role = "User",
            //    //CreatedAt = DateTime.UtcNow,
            //}]);

            base.Configure(builder);
        }
    }
}

