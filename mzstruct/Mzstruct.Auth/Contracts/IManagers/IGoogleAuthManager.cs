using System;
using System.Collections.Generic;
using System.Text;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Mzstruct.Auth.Contracts.IManagers
{
    public interface IGoogleAuthManager
    {
        Task<Payload?> ValidateToken(string credential);
    }
}
