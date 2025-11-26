using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Domain.Errors
{
    public record Error(ErrorType type, string code, string message, string title = "", string detail = "")
    {
        public static Error None => new(ErrorType.None, string.Empty, string.Empty);
        public static Error Validation => new(ErrorType.Validation, string.Empty, string.Empty);
        public static Error Invalid => new(ErrorType.Invalid, string.Empty, string.Empty);
        public static Error NotFound => new(ErrorType.NotFound, string.Empty, string.Empty);
        public static Error Missing => new(ErrorType.Missing, string.Empty, string.Empty);
        public static Error Conflict => new(ErrorType.Conflict, string.Empty, string.Empty);
    }

    public enum ErrorType
    {
        None,
        Validation,
        Invalid,
        NotFound,
        Missing,
        Conflict,
        Network,
    }
}
