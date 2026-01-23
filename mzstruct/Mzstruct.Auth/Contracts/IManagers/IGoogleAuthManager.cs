using Microsoft.AspNetCore.Authentication;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Mzstruct.Auth.Contracts.IManagers
{
    public interface IGoogleAuthManager
    {
        Task<BaseUserModel?> ValidateClaim(AuthenticateResult authResult);
        Task<Payload?> ValidateToken(string credential);
    }
}
