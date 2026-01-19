using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Mzstruct.Auth.Contracts.IManagers;
using Mzstruct.DB.Contracts.IContext;
using System.Security.Claims;

namespace Mzstruct.Auth.Managers
{
    public class IdentityAuthManager<TIdentity>(IAppDBContext appDBContext,
        UserManager<TIdentity> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<TIdentity> signInManager,
        JwtTokenManager jwtTokenManager) : IIdentityAuthManager<TIdentity> where TIdentity : IdentityUser, new()
    {
        public async Task<string> Register(RegisterRequest req)
        {
            var user = new TIdentity
            {
                UserName = req.Email,
                Email = req.Email
            };
            var defaultRole = "User";
            var claimType = "Permission";
            List<string> accessLevels = ["Read", "Update", "Delete"];

            var existingRole = await roleManager.FindByNameAsync(defaultRole);
            if (existingRole is null)
            {
                await roleManager.CreateAsync(existingRole = new IdentityRole(defaultRole));
                foreach (var access in accessLevels)
                {
                    await roleManager.AddClaimAsync(existingRole, new Claim(claimType, defaultRole + ":" + access));
                }
                //await roleManager.AddClaimAsync(existingRole, new Claim(claimType, defaultRole + ":Read"));
                //await roleManager.AddClaimAsync(existingRole, new Claim(claimType, defaultRole + ":Delete"));
            }
                
            using var transaction = await appDBContext.Database.BeginTransactionAsync();         
            var result = await userManager.CreateAsync(user, req.Password);
            if (!result.Succeeded)
                return string.Join(", ", result.Errors.Select(e => e.Description));   
            await userManager.AddToRoleAsync(user, defaultRole); // Optional: assign default role
            var roles = await userManager.GetRolesAsync(user);
            var token = jwtTokenManager.CreateIdentityToken(user, [defaultRole]);
            await transaction.CommitAsync();
            return token;
        }

        public async Task<string> Login(LoginRequest req)
        {
            var user = await userManager.FindByEmailAsync(req.Email);
            if (user is null) return "Invalid credentials";

            var isSucceeded = await userManager.CheckPasswordAsync(user, req.Password);
            if (!isSucceeded) return "Invalid credentials";

            var check = await signInManager.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: true);
            if (!check.Succeeded) return "Invalid credentials";
          
            var roles = await userManager.GetRolesAsync(user);

            //var claimType = "Permission";
            //var parmissions = await (from role in appDBContext.Roles
            //                         join claim in appDBContext.RoleClaims on role.Id equals claim.RoleId
            //                         where roles.Contains(role.Name) && claim.ClaimType == claimType
            //                          select claim.ClaimValue).Distinct().ToListAsync();
            //var permissionClaims = parmissions.Select(p => new Claim(claimType, p)).ToList();

            var token = jwtTokenManager.CreateIdentityToken(user, roles); //, permissionClaims
            return token;
        }
    }
}
