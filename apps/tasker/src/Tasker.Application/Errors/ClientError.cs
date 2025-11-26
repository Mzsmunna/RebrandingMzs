using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Errors
{
    public static class ClientError
    {
        public static readonly Error BadRequest = new(ErrorType.Bad, "Client.Request.Bad", "", 400, "https://www.rfc-editor.org/rfc/rfc9110#name-400-bad-request");
    }
}
