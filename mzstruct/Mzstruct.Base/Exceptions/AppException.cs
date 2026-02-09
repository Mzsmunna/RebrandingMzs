using Mzstruct.Base.Patterns.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Exceptions
{
    public class AppException : Exception
    {
        public Error Error { get; }
        public AppException(Error error) : base(error.Message)
        {
            Error = error;
        }
    }
}
