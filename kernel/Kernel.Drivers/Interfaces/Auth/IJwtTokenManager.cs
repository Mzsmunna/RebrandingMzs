using Kernel.Drivers.Dtos;
using Kernel.Drivers.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Kernel.Drivers.Interfaces.Auth
{
    public interface IJwtTokenManager
    {
        public RefreshToken GenerateRefreshToken();
        public void SetRefreshToken(RefreshToken newRefreshToken, Identity user, ControllerBase? controller = null);
        public string CreateToken(Identity user, List<Claim>? additionalClaims = null);
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
