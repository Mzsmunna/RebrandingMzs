using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Exceptions
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

            var problem = new ProblemDetails
            {
                //Instance = $"{httpContext.Request.Method} {httpContext.Request.Path.Value}",
                Type = ex.GetType().Name,  //"https://www.rfc-editor.org/rfc/rfc9110#name-500-internal-server-error",
                Title = "Internal server error!!",
                Status = StatusCodes.Status500InternalServerError,
                Detail = ex.Message,
                //Extensions = new Dictionary<string, object?>
                //{
                //    { "requestId", httpContext.TraceIdentifier },
                //    { "traceId", activity?.Id }
                //}
            };

            if (ex is AppException appEx)
            {
                problem = new ProblemDetails
                {
                    Title = appEx.Error.Title,
                    Detail = appEx.Error.Message,
                    Status = appEx.Error.StatusCode,
                    //Instance = httpContext.Request.Path
                };

                //if (ex is ValidationException validationEx)
                if (appEx.Error.Details is not null && appEx.Error.Details.Count > 0)
                {
                    problem.Extensions = appEx.Error.Details.
                    ToDictionary(
                        kvp => kvp.Key,
                        kvp => (object?)kvp.Value
                    );
                }
                httpContext.Response.StatusCode = appEx.Error.StatusCode;
            }
            else
            {
                httpContext.Response.StatusCode = ex switch
                {
                    ApplicationException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };
            }

            //var activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;
            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = ex,
                //StatusCode = httpContext.Response.StatusCode,
                ProblemDetails = problem
            });
        }
    }
}
