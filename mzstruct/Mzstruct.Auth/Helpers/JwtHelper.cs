using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mzstruct.Auth.Configs;
using Mzstruct.Auth.Models;
using Mzstruct.Base.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Mzstruct.Auth.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateJwtToken(string secretKey, object? payload)
        {
            if (string.IsNullOrEmpty(secretKey)) return string.Empty;
            JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>();

            if (payload != null)
            {
                foreach (PropertyInfo claimInfo in payload.GetType().GetProperties())
                {
                    claims.Add(new Claim(claimInfo.Name, claimInfo.GetValue(payload, null)?.ToString() ?? ""));
                }
            }          

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials
            };

            SecurityToken securityToken = _tokenHandler.CreateToken(securityTokenDescriptor);
            string token = _tokenHandler.WriteToken(securityToken);
            return token;
        }

        public static RefreshToken GenerateRefreshToken(JwtTokenOptions? options = null)
        {
            if (options is null) options = new();
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = BaseHelper.ToDateTime(options.RefreshTokenExpiryValue, options.RefreshTokenExpiryUnit), //DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
            return refreshToken;
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public static string GetValueFromToken(string token, string key)
        {
            var securityToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            if (securityToken is null) return "";
            var claim = securityToken.Claims.FirstOrDefault(claim => claim.Type == key);
            return (claim != null) ? claim.Value : "";
        }
    }
}
