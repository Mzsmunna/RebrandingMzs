using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Mzstruct.Base.Helpers;
using Mzstruct.Auth.Contracts.IManagers;
using Mzstruct.Auth.Models;
using Mzstruct.Auth.Configs;

namespace Mzstruct.Auth.Managers
{
    public class JwtTokenManager(IConfiguration config, 
        IOptions<JwtTokenOptions> options, 
        IHttpContextAccessor httpContextAccessor) : IJwtTokenManager
    {
        //private readonly JwtTokenOptions _options = options.Value;
        //private readonly IConfiguration _config = config;

        public RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = BaseHelper.ToDateTime(options.Value.RefreshTokenExpiryValue, options.Value.RefreshTokenExpiryUnit), //DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
            return refreshToken;
        }

        public void SetRefreshToken(RefreshToken newRefreshToken, Identity user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            if (httpContextAccessor.HttpContext is not null)
                httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }

        public string CreateToken(Identity user, List<Claim>? additionalClaims = null)
        {
            var tokenExpiredOn = BaseHelper.ToDateTime(options.Value.TokenExpiryValue, options.Value.TokenExpiryUnit); //DateTime.UtcNow.AddMinutes(15);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email.ToLower()),
                new Claim(ClaimTypes.Role, user.Role.ToLower()),
                new Claim(ClaimTypes.Expiration, tokenExpiredOn.ToString())
            };

            if (additionalClaims is not null && additionalClaims.Any())
                claims.AddRange(additionalClaims);

            SymmetricSecurityKey? key = null;
            var secret = !string.IsNullOrEmpty(options.Value.SecretKey) ? options.Value.SecretKey : config.GetValue<string>(options.Value.SecretConfigKey);
            
            if (!string.IsNullOrEmpty(secret))
                key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            else
                throw new Exception("JWT secret key not provided.");

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: tokenExpiredOn,
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
