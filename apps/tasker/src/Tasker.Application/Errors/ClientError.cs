using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Errors
{
    public static class ClientError
    {
        public static readonly Error InvalidRequest = new(ErrorType.Invalid, "Invalid_Request", "");
    }
}
