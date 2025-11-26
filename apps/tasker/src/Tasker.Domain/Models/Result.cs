using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tasker.Domain.Models
{
    public class Result<T>
    {
        private readonly T _value;
        private readonly Error _error;
        private readonly bool _isSuccess;

        private Result(T value)
        {
            _isSuccess = true; 
            _value = value;
            _error = Error.None;
        }

        private Result(Error error)
        {
            _isSuccess = false; 
            _value = default;
            _error = error;
        }

        public static implicit operator Result<T>(T value) => new(value);
        public static implicit operator Result<T>(Error error) => new(error);

        public static Result<T> Success(T value) => new Result<T>(value);
        public static Result<T> Failure(Error error) => new Result<T>(error);
        public TResult Map<TResult>(
            Func<T, TResult> success,
            Func<Error, TResult> failure)
        {
            return _isSuccess ? success(_value) : failure(_error);
        }
    }
}
