using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Errors
{
    public static class AppError
    {
        public static readonly Error ServerError = new(ErrorType.Server, "Server.Error", "", 500);
    }
}
