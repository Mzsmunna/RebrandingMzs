using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Errors
{
    public static class ClientError
    {
        public static readonly Error BadRequest = new(ErrorType.Bad, "Client.Bad_Request", "", 400);
    }
}
