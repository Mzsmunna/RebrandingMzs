using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Mzstruct.Auth.Models;
using Mzstruct.Auth.Models.Configs;
using Mzstruct.Base.Consts;
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
                ExpiresAt = BaseHelper.ToDateTime(options.RefreshTokenExpiryValue, options.RefreshTokenExpiryUnit), //DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
            return refreshToken;
        }

        public static string GetValueFromToken(string token, string key)
        {
            var securityToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            if (securityToken is null) return "";
            var claim = securityToken.Claims.FirstOrDefault(claim => claim.Type == key);
            return (claim != null) ? claim.Value : "";
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey(JwtTokenOptions options, IConfiguration config)
        {
            //var config = AppConst.GetConfig();
            string secret = options.SecretKey ??
                            config.GetValue<string>(options.SecretConfigKey ?? "JWTAuthSecretKey")
                            ?? throw new Exception("JWT secret key not provided.");
            return GetSymmetricSecurityKey(secret);
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey(string secret)
        {
            if (string.IsNullOrEmpty(secret)) throw new Exception("JWT secret key not provided.");
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            return signingKey;
        }

        public static List<Claim> GetClaims(List<Claim>? additionalClaims = null)
        {
            var jti = Guid.NewGuid().ToString();
            List<Claim> claims = new List<Claim>();            
            if (additionalClaims != null && additionalClaims.Any())
            {
                if (!claims.Any(c => c.Type == JwtRegisteredClaimNames.Jti))
                {
                    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));
                }
                claims.AddRange(additionalClaims);
            }
            else claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));        
            return claims;
        }

        public static TokenValidationParameters GetTokenValidationParameters(SymmetricSecurityKey signingKey, JWTAuth? config)
        {
            return new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero, //TimeSpan.FromSeconds(30)
                    IssuerSigningKey = signingKey,
                    ValidIssuer = config?.Issuer,
                    ValidAudience = config?.Audience,
                };
        }

        public static JwtBearerEvents GetJwtBearerEvents(string requestQuery = "access_token", string segment = "/jwtevents")
        {
            return new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query[requestQuery];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) 
                        && (path.StartsWithSegments(segment))) context.Token = accessToken;
                        return Task.CompletedTask;
                    }
                };
        }
    }
}
