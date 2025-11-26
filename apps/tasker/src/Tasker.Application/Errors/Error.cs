using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Errors
{
    public record Error(ErrorType Type, string Code, string Message, int StatusCore = 102, string Url = "", string Title = "", string Detail = "")
    {
        public static Error None => new(ErrorType.None, string.Empty, string.Empty);
        public static Error Validation(string code = "", string message = "") => new(ErrorType.Validation, code, message, 422, "https://www.rfc-editor.org/rfc/rfc9110#name-422-unprocessable-content");
        public static Error Bad(string code = "", string message = "") => new(ErrorType.Bad, code, message, 400, "https://www.rfc-editor.org/rfc/rfc9110#name-400-bad-request");
        public static Error NotFound(string code = "", string message = "") => new(ErrorType.NotFound, code, message, 404, "https://www.rfc-editor.org/rfc/rfc9110#name-404-not-found");
        public static Error Missing(string code = "", string message = "") => new(ErrorType.Missing, code, message, 404, "https://www.rfc-editor.org/rfc/rfc9110#name-404-not-found");
        public static Error Conflict(string code = "", string message = "") => new(ErrorType.Conflict, code, message, 409, "https://www.rfc-editor.org/rfc/rfc9110#name-409-conflict");
        public static Error Network(string code = "", string message = "") => new(ErrorType.Network, code, message, 502, "https://www.rfc-editor.org/rfc/rfc9110#name-502-bad-gateway");
        public static Error Server(string code = "", string message = "") => new(ErrorType.Server, code, message, 500, "https://www.rfc-editor.org/rfc/rfc9110#name-500-internal-server-error");
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
