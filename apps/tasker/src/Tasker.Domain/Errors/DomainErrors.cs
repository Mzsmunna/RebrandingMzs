using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Domain.Errors
{
    public static class DomainErrors
    {
        public static readonly Error MissingId = new(ErrorType.Missing, "ID_Missing", "");
        public static readonly Error InvalidRequest = new(ErrorType.Invalid, "Invalid_Request", "");
    }
}
