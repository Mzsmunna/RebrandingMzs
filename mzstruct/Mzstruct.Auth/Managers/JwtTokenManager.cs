using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mzstruct.Auth.Contracts.IManagers;
using Mzstruct.Auth.Helpers;
using Mzstruct.Auth.Models;
using Mzstruct.Auth.Models.Configs;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mzstruct.Auth.Managers
{
    public class JwtTokenManager(IConfiguration config, 
        IOptions<JwtTokenOptions> options, 
        IHttpContextAccessor httpContextAccessor) : IJwtTokenManager
    {
        //private readonly JwtTokenOptions _options = options.Value;
        //private readonly IConfiguration _config = config;

        public string CreateNewToken(Identity? user = null, List<Claim>? claims = null)
        {
            claims = JwtHelper.GetClaims(claims);
            if (user != null)
            {
                List<Claim> userClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Surname, user.Name ?? ""),
                    new Claim(ClaimTypes.Email, user.Email.ToLower()),
                    new Claim(ClaimTypes.Role, user.Role.ToLower()),
                };
                claims.AddRange(userClaims);
            }
            string jwt = CreateToken(claims);
            SetRefreshToken(user);
            return jwt;
        }

        public string CreateIdentityToken<TIdentity>(TIdentity user, IList<string> roles, List<Claim>? additionalClaims = null) where TIdentity : IdentityUser
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.Surname, user.UserName ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email?.ToLower() ?? ""),
                //new Claim(ClaimTypes.Role, user.Role.ToLower()),
            };
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));
            SetRefreshToken();
            return CreateToken(claims);
        }

        public string CreateToken(List<Claim>? claims = null)
        {
            var tokenExpiringAt = BaseHelper.ToDateTime(options.Value.TokenExpiryValue, options.Value.TokenExpiryUnit);
            if (claims is null) claims = JwtHelper.GetClaims(claims);
            if (!claims.Any(c => c.Type == ClaimTypes.Expiration))
            {
                claims.Add(new Claim(ClaimTypes.Expiration, tokenExpiringAt.ToString() ?? DateTime.UtcNow.AddMinutes(15).ToString()));
            }  
            var key = JwtHelper.GetSymmetricSecurityKey(options.Value, config);         
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                issuer: options.Value.jwtAuthConfig?.Issuer,
                audience: options.Value.jwtAuthConfig?.Audience, // ✅ must match config
                claims: claims,
                expires: tokenExpiringAt,
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public RefreshToken SetRefreshToken(Identity? user = null)
        {
            var newRefreshToken = JwtHelper.GenerateRefreshToken(options.Value);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.ExpiresAt
            };

            if (httpContextAccessor.HttpContext is not null)
                httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            if (user != null)
            {
                user.RefToken = newRefreshToken;
                user.RefreshToken = newRefreshToken.Token;
                user.TokenCreated = newRefreshToken.CreatedAt;
                user.TokenExpires = newRefreshToken.ExpiresAt;
            }
            
            return newRefreshToken;
        }

        public bool ValidateToken(string? token = "")
        {
            try
            {
                //var principal = handler.ValidateToken(token, parameters, out var validatedToken);
                var (principal, validatedToken) = GetPrincipalFromToken(token);
                
                if (validatedToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase)) //StringComparison.InvariantCultureIgnoreCase
                {
                    return false;
                }

                if (principal is null || jwtToken is null)
                    return false;

                return jwtToken.ValidTo <= DateTime.UtcNow;
            }
            catch
            {
                return false;
            }
        }

        public (ClaimsPrincipal?, SecurityToken?) GetPrincipalFromToken(string? token = "")
        {
            if (string.IsNullOrEmpty(token) && httpContextAccessor.HttpContext is not null)
                token = httpContextAccessor.HttpContext.Request.Headers.Authorization
                .FirstOrDefault()?
                .Replace("Bearer ", "") ?? "";

            var key = JwtHelper.GetSymmetricSecurityKey(options.Value, config);
            var _baseParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = options.Value.jwtAuthConfig?.Issuer ?? "",
                ValidAudience = options.Value.jwtAuthConfig?.Audience ?? "",
                // IMPORTANT: we’ll override ValidateLifetime per call
                ClockSkew = TimeSpan.Zero
            };

            var parameters = _baseParams.Clone();
            parameters.ValidateLifetime = false; // allow expired
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var principal = handler.ValidateToken(token, parameters, out var validatedToken);
                return (principal, validatedToken);
            }
            catch
            {
                return (null, null);
            }
        }

        public string GetValueFromToken(string token, string key) => JwtHelper.GetValueFromToken(token, key);
        public string CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) => PasswordHelper.HashWithHMACSHA512(password, out passwordHash, out passwordSalt);
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) => PasswordHelper.VerifyWithHMACSHA512(password, passwordHash, passwordSalt);
    }
}
