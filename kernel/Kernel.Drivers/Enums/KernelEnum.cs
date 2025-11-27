using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Drivers.Enums
{
    public enum CasingType
    {
        ToLowerCase,
        ToUpperCase,
        ToTitleCase
    }

    public enum ErrorType
    {
        None,
        Bad,
        Unauthorized,
        Forbidden,
        NotFound,
        Missing,
        Conflict,
        Validation,
        RateLimit,
        Server,
        Network
    }
}
