using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Errors;
using Tasker.Application.Models;

namespace Tasker.Application.Extensions
{
    public static class ErrorExtender
    {
        public static IResult ToProblemDetails<T>(this Result<T> result)
        {
            if (result.IsSuccess)
                throw new InvalidOperationException($"Success result shouldn't return any problem+json response");
            return result.Error.ToProblemDetails();
        }

        public static IResult ToProblemDetails(this Error error)
        {
            return Results.Problem(
                type: error.Url,
                title: error.Title,
                detail: error.Message,
                statusCode: error?.StatusCore ?? StatusCodes.Status500InternalServerError,
                extensions: new Dictionary<string, object?>
                {
                    { "errors", error }
                }
            );
        }
    }
}
