using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Mzstruct.Auth.Contracts.IHandlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Interceptors
{
    public class CustomAuthorizationRequirement : IAuthorizationRequirement
    {    
        public CustomAuthorizationRequirement() { }
    }

    public class CustomAuthorizationHandler(ICustomAuthorizer customAuthorizer, ILogger<CustomAuthorizationHandler> logger) : AuthorizationHandler<CustomAuthorizationRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement)
        {
            try
            {
                if (context != null && context.Resource != null && context.Resource is DefaultHttpContext filterContext)
                {
                    var httpContext = filterContext.HttpContext;
                    string requestedUrl = string.Empty;
                    if (httpContext != null && httpContext.Request != null && httpContext.Request.Path != null)
                    {
                        requestedUrl = httpContext.Request.Path.ToString().ToLower();
                        if (!string.IsNullOrEmpty(requestedUrl) && await customAuthorizer.ValidatePermissions(context, httpContext, requestedUrl))
                            context.Succeed(requirement);
                        else context.Fail();
                    }
                    else context.Fail();
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("CustomAuthorizationHandler Exception: " + ex.Message);
                context.Fail();
            }
            finally { }
            return;
        }
    }
}
