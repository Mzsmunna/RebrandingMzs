using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Exceptions
{
    public class MiddlewareExceptionHandler(
        RequestDelegate next,
        ILogger<MiddlewareExceptionHandler> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            { 
                logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                var problemDetails = new ProblemDetails
                {
                    //Instance = httpContext.Request.Path.Value,
                    Type = "https://www.rfc-editor.org/rfc/rfc9110#name-500-internal-server-error",
                    Title = "Server error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = ex.Message
                };
                context. Response. StatusCode = StatusCodes. Status500InternalServerError;
                await context. Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
