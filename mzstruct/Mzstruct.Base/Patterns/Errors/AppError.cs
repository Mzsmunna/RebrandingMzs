using Mzstruct.Base.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Patterns.Errors
{
    public static class AppError
    {
        public static readonly Error ServerError = new(ErrorType.Server, "Server.Error", "", 500);
    }
}
