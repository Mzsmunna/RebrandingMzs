using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Extensions
{
    public static class ErrorExtender
    {
        public static IResult ToProblemDetails<T>(this Result<T> result)
        {
            if (result.IsSuccess)
                throw new InvalidOperationException($"Success result shouldn't return any problem+json response");
            return result.Error.ToProblemDetails();
            //return Results.Problem(
            //    type: error.Url,
            //    title: error.Title,
            //    detail: error.Message,
            //    statusCode: error?.StatusCore ?? StatusCodes.Status500InternalServerError,
            //    extensions: new Dictionary<string, object?>
            //    {
            //        { "errors", new[] { result.Error } }
            //    }
            //);
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
                type: error.Code, //.Type.ToString(),
                title: error.Title ?? "Something went wrong!!",
                detail: error.Message,
                statusCode: error?.StatusCore ?? StatusCodes.Status500InternalServerError,
                extensions: new Dictionary<string, object?>
                {
                    //{ "requestId", httpContext.TraceIdentifier },
                    //{ "traceId", activity?.Id }
                    { "errors", error }
                }
            );
        }

        public static Dictionary<string, string[]>? ToErrorDictionary(this ValidationResult validation)
        {
            if (validation.IsValid is false)
            {
                var errors = validation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
                return errors;
            }
            return null;
        }
    }
}
