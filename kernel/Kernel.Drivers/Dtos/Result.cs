using Kernel.Drivers.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Drivers.Dtos
{
    public class Result<T>
    {
        public readonly T Data;
        public readonly Error Error;
        public readonly bool IsSuccess;

        private Result(T data)
        {
            IsSuccess = true; 
            Data = data;
            Error = Error.None;
        }

        private Result(Error error)
        {
            IsSuccess = false; 
            Data = default!;
            Error = error;
        }

        public static implicit operator Result<T>(T data) => new(data);
        public static implicit operator Result<T>(Error error) => new(error);

        public static Result<T> Ok(T data) => new Result<T>(data);
        public static Result<T> Err(Error error) => new Result<T>(error);
        public TResult Map<TResult>(
            Func<T, TResult> Ok,
            Func<Error, TResult> Err)
        {
            return IsSuccess ? Ok(Data) : Err(Error);
        }
    }
}
