using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Tasker.Application
{
    internal sealed class GlobalExceptionHandler(
        IProblemDetailsService problemDetailsService,
        ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            // Log the exception details here
            const string error = "An unhandled error occurred while processing the request!";
            logger.LogError(exception, error);
            httpContext.Response.StatusCode = exception switch
            {
                ApplicationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                //StatusCode = httpContext.Response.StatusCode,
                ProblemDetails = new ProblemDetails
                {
                    Type = exception.GetType().Name,
                    Title = error,
                    Status = httpContext.Response.StatusCode,
                    Detail = exception.Message
                }
            });
        }
    }
}
