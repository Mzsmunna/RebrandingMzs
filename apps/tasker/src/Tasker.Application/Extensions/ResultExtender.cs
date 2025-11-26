using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Models;

namespace Tasker.Application.Extensions
{
    public static class ResultExtender
    {
        public static IResult ToProblemDetails<T>(this Result<T> result)
        {
            if (result.IsSuccess)
                throw new InvalidOperationException($"Success result shouldn't return any problem+json response");
            return Results.Problem(
                type: result.Error.Url,
                title: result.Error.Title,
                detail: result.Error.Message,
                statusCode: result.Error?.StatusCore ?? StatusCodes.Status500InternalServerError,
                extensions: new Dictionary<string, object?>
                {
                    { "errors", result.Error }
                }
            );
        }
    }
}
