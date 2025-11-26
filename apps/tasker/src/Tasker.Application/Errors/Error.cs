using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Errors
{
    public record Error(ErrorType Type, string Code, string Message, int StatusCore = 102, string Title = "", string Detail = "", string Url = "")
    {
        public static Error None => new(ErrorType.None, string.Empty, string.Empty);
        public static Error Validation(string code = "", string message = "") => new(ErrorType.Validation, code, message);
        public static Error Invalid(string code = "", string message = "") => new(ErrorType.Invalid, code, message);
        public static Error NotFound(string code = "", string message = "") => new(ErrorType.NotFound, code, message);
        public static Error Missing(string code = "", string message = "") => new(ErrorType.Missing, code, message);
        public static Error Conflict(string code = "", string message = "") => new(ErrorType.Conflict, code, message);
        public static Error Network(string code = "", string message = "") => new(ErrorType.Network, code, message);
        public static Error Server(string code = "", string message = "") => new(ErrorType.Server, code, message);
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
        Server
    }
}
