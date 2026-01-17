using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Mzstruct.Auth.Configs;
using Mzstruct.Auth.Contracts.IManagers;
using Mzstruct.DB.Contracts.IContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Managers
{
    public class IdentityAuthManager<TIdentity>(IAppDBContext appDBContext,
        UserManager<TIdentity> userManager,
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

            using var transaction = await appDBContext.Database.BeginTransactionAsync();       
            var result = await userManager.CreateAsync(user, req.Password);
            if (!result.Succeeded)
                return string.Join(", ", result.Errors.Select(e => e.Description));
      
            await userManager.AddToRoleAsync(user, "User"); // Optional: assign default role
            var roles = await userManager.GetRolesAsync(user);
            var token = jwtTokenManager.CreateIdentityToken(user, ["User"]);
            await transaction.CommitAsync();
            return token;
        }

        public async Task<string> Login(LoginRequest req)
        {
            var user = await userManager.FindByEmailAsync(req.Email);
            if (user is null) return "Invalid credentials";

            var check = await signInManager.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: true);
            if (!check.Succeeded) return "Invalid credentials";

            var roles = await userManager.GetRolesAsync(user);
            var token = jwtTokenManager.CreateIdentityToken(user, roles);
            return token;
        }
    }
}
