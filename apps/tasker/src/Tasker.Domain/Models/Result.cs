using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Domain.Models
{
    public class Result<T>
    {
        private readonly T _data;
        private readonly Error _error;
        private readonly bool _isSuccess;

        private Result(T data)
        {
            _isSuccess = true; 
            _data = data;
            _error = Error.None!;
        }

        private Result(Error error)
        {
            _isSuccess = false; 
            _data = default!;
            _error = error;
        }

        public static implicit operator Result<T>(T data) => new(data);
        public static implicit operator Result<T>(Error error) => new(error);

        public static Result<T> Ok(T data) => new Result<T>(data);
        public static Result<T> Err(Error error) => new Result<T>(error);
        public TResult Map<TResult>(
            Func<T, TResult> Ok,
            Func<Error, TResult> Err)
        {
            return _isSuccess ? Ok(_data) : Err(_error);
        }
    }
}
