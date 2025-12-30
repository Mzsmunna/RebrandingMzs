using Microsoft.AspNetCore.Mvc;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Common.Extensions
{
    public static class ResultExtender
    {
        public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
        {
            return result.Map<IActionResult>(
                Ok: data => controller.Ok(data),
                Err: error => error.ToProblem(controller)
            );
        }

        public static Result<TOut> To<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mapper)
        {
            if (result.IsSuccess)
                return Result<TOut>.Ok(mapper(result.Data));
            return Result<TOut>.Err(result.Error);
        }

        public static Result<TOut> Include<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> next)
        {
            return result.IsSuccess ? next(result.Data) : result.Error;
        }

        public static Result<TOut> TryCatch<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> func, Error error)
        { 
            try
            {
                return result.IsSuccess? func(result.Data) : result.Error;
            }
            catch
            {
                return error;
            }
        }

        public static Result<TIn> Execute<TIn>(this Result<TIn> result, Action<TIn> action)
        {
            if (result.IsSuccess)
                action(result.Data);
            return result;
        }

        public static TOut Match<TIn, TOut>(
            this Result<TIn> result,
            Func<TIn, TOut> Ok,
            Func<Error, TOut> Err)
        {
            return result.IsSuccess ? Ok(result.Data) : Err(result.Error);
        }
    }
}
