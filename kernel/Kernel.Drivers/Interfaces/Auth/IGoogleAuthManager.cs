using System;
using System.Collections.Generic;
using System.Text;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Kernel.Drivers.Interfaces.Auth
{
    public interface IGoogleAuthManager
    {
        Task<Payload?> ValidateToken(string credential);
    }
}
