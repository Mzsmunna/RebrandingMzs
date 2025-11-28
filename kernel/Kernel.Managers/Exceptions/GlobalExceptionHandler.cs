using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Managers.Exceptions
{
    public sealed class GlobalExceptionHandler(
        IProblemDetailsService problemDetailsService,
        ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception ex, 
            CancellationToken cancellationToken)
        {
            // Log the exception details here
            logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            
            httpContext.Response.StatusCode = ex switch
            {
                ApplicationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = ex,
                //StatusCode = httpContext.Response.StatusCode,
                ProblemDetails = new ProblemDetails
                {
                    Instance = httpContext.Request.Path.Value,
                    Type = "https://www.rfc-editor.org/rfc/rfc9110#name-500-internal-server-error",
                    Title = "Server error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = ex.Message
                }
            });
        }
    }
}
