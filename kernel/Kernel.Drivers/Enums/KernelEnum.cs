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

    public enum TimeUnit
    {
        Seconds = 1,
        Minutes = 1 * 60,
        Hours = 1 * 60 * 60,
        Days = 1 * 60 * 60 * 24,
        Weeks = 1 * 60 * 60 * 24 * 7,
        Months = 1 * 60 * 60 * 24 * 30,
        Years = 1 * 60 * 60 * 24 * 365
    }

}
