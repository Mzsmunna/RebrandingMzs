using Kernel.Drivers.Dtos;
using Kernel.Drivers.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Managers.Extensions
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

        public static ObjectResult ToProblem<T>(this Result<T> result, ControllerBase controller)
        {
            if (result.IsSuccess)
                throw new InvalidOperationException($"Success result shouldn't return any problem+json response");
            return result.Error.ToProblem(controller);
        }

        public static ObjectResult ToProblem(this Error error, ControllerBase controller)
        {
            return controller.Problem(
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
