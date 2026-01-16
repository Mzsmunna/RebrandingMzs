using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Mzstruct.Auth.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Contracts.IManagers
{
    public interface IIdentityAuthManager<TIdentity> where TIdentity : IdentityUser, new()
    {
        Task<string> Register(RegisterRequest req);
        Task<string> Login(LoginRequest req);
    }
}
