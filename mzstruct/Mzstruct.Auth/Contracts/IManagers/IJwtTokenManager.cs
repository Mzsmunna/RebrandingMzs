using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mzstruct.Auth.Models;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Mzstruct.Auth.Contracts.IManagers
{
    public interface IJwtTokenManager
    {
        public RefreshToken GenerateRefreshToken();
        public void SetRefreshToken(RefreshToken newRefreshToken, Identity user);
        public string CreateIdentityToken<TIdentity>(TIdentity user, IList<string> roles, List<Claim>? additionalClaims = null) where TIdentity : IdentityUser;
        public string CreateToken(Identity? user = null, List<Claim>? additionalClaims = null);
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        public string GetValueFromToken(string token, string key);
    }
}
