using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mzstruct.Auth.Contracts.IManagers;
using Mzstruct.Auth.Helpers;
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

        public string CreateNewToken(Identity? user = null, List<Claim>? additionalClaims = null)
        {
            string jwt = CreateToken(user, additionalClaims);
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
            return CreateToken(null, claims);
        }

        public string CreateToken(Identity? user = null, List<Claim>? additionalClaims = null)
        {     
            var tokenExpiredOn = BaseHelper.ToDateTime(options.Value.TokenExpiryValue, options.Value.TokenExpiryUnit); //DateTime.UtcNow.AddMinutes(15);          
            List<Claim> claims = new List<Claim>();
            var key = JwtHelper.GetSymmetricSecurityKey(options.Value, config);
            if (key is null) 
                throw new Exception("JWT secret key not provided.");

            if (user != null) 
            {
                List<Claim> userClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Surname, user.Name),
                    new Claim(ClaimTypes.Email, user.Email.ToLower()),
                    new Claim(ClaimTypes.Role, user.Role.ToLower()),
                };
                claims.AddRange(userClaims);
            }
            
            if (additionalClaims is not null && additionalClaims.Any())
                claims.AddRange(additionalClaims);
            claims.Add(new Claim(ClaimTypes.Expiration, tokenExpiredOn.ToString()));
            
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: tokenExpiredOn,
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public void SetRefreshToken(Identity? user)
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
                user.RefreshToken = newRefreshToken.Token;
                user.TokenCreated = newRefreshToken.CreatedAt;
                user.TokenExpires = newRefreshToken.ExpiresAt;
            }
        }

        public bool ValidateToken(string? token = "")
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
                if (validatedToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
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

        public string GetValueFromToken(string token, string key) => JwtHelper.GetValueFromToken(token, key);
        public string CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) => PasswordHelper.HashWithHMACSHA512(password, out passwordHash, out passwordSalt);
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) => PasswordHelper.VerifyWithHMACSHA512(password, passwordHash, passwordSalt);
    }
}
