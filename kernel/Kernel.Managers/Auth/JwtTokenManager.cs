using Kernel.Drivers.Dtos;
using Kernel.Drivers.Entities;
using Kernel.Drivers.Models;
using Kernel.Helpers;
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

namespace Kernel.Managers.Auth
{
    public class JwtTokenManager
    {
        private readonly JwtTokenOptions _options;
        private readonly IConfiguration _config;

        public JwtTokenManager(IConfiguration config, IOptions<JwtTokenOptions> options)
        {
            _options = options.Value;
            _config = config;
        }

        public RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = SharedHelperUtility.ToDateTime(_options.RefreshTokenExpiryValue, _options.RefreshTokenExpiryUnit), //DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
            return refreshToken;
        }

        public void SetRefreshToken(RefreshToken newRefreshToken, Identity user, ControllerBase? controller = null)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            if (controller != null)
                controller.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }

        public string CreateToken(Identity user, List<Claim>? additionalClaims = null)
        {
            var tokenExpiredOn = SharedHelperUtility.ToDateTime(_options.TokenExpiryValue, _options.TokenExpiryUnit); //DateTime.UtcNow.AddMinutes(15);

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
            var secret = !string.IsNullOrEmpty(_options.SecretKey) ? _options.SecretKey : _config.GetValue<string>(_options.SecretConfigKey);
            
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
