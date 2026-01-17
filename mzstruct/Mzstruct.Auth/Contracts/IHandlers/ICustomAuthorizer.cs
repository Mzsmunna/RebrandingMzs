using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Contracts.IHandlers
{
    public interface ICustomAuthorizer
    {
        public Task<bool> ValidatePermissions(AuthorizationHandlerContext context, HttpContext httpContext, string requestedUrl);
    }
}
