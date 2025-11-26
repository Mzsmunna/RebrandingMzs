using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Errors
{
    public record Error(ErrorType Type, string Code, string Message, int StatusCore = 102, string Title = "", string Detail = "", string Url = "")
    {
        public static Error None => new(ErrorType.None, string.Empty, string.Empty);
        public static Error Validation(string code = "", string message = "") => new(ErrorType.Validation, code, message);
        public static Error Bad(string code = "", string message = "") => new(ErrorType.Bad, code, message, 400);
        public static Error NotFound(string code = "", string message = "") => new(ErrorType.NotFound, code, message, 404);
        public static Error Missing(string code = "", string message = "") => new(ErrorType.Missing, code, message, 404);
        public static Error Conflict(string code = "", string message = "") => new(ErrorType.Conflict, code, message, 409);
        public static Error Network(string code = "", string message = "") => new(ErrorType.Network, code, message);
        public static Error Server(string code = "", string message = "") => new(ErrorType.Server, code, message, 500);
    }

    public enum ErrorType
    {
        None,
        Validation,
        Bad,
        NotFound,
        Missing,
        Conflict,
        Network,
        Server
    }
}
