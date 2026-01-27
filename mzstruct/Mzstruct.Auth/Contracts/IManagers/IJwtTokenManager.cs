using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        public string CreateNewToken(Identity? user = null, List<Claim>? additionalClaims = null);
        public string CreateIdentityToken<TIdentity>(TIdentity user, IList<string> roles, List<Claim>? additionalClaims = null) where TIdentity : IdentityUser;
        public string CreateToken(List<Claim>? additionalClaims = null);
        public RefreshToken SetRefreshToken(Identity? user = null);
        public bool ValidateToken(string? token = "");
        public (ClaimsPrincipal?, SecurityToken?) GetPrincipalFromToken(string? token = "");
        public string GetHeaderToken();
        public string GetHeaderRefreshToken();
        public string GetValueFromToken(string token, string key);
        public string CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
