using Mzstruct.Base.Patterns.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Patterns.Result
{
    public class Result<TValue>
    {
        public readonly TValue Data;
        public readonly Error Error;
        public readonly bool IsSuccess;

        private Result(TValue data)
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

        public static implicit operator Result<TValue>(TValue data) => new(data);
        public static implicit operator Result<TValue>(Error error) => new(error);

        public static Result<TValue> Ok(TValue data) => new Result<TValue>(data);
        public static Result<TValue> Err(Error error) => new Result<TValue>(error);
        public TResponse Map<TResponse>(
            Func<TValue, TResponse> Ok,
            Func<Error, TResponse> Err)
        {
            return IsSuccess ? Ok(Data) : Err(Error);
        }
    }
}
