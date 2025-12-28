using Mzstruct.Base.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Errors
{
    public sealed record Error(ErrorType Type, string Code, string Message, int StatusCore = 102, string Url = "", string Title = "", string Detail = "", IDictionary<string, string[]>? Errors = null)
    {
        public static readonly Error None = new(ErrorType.None, string.Empty, string.Empty);
        
        public static Error Null(string code = "", string message = "") => new(ErrorType.Null, code, message, 204, "https://www.rfc-editor.org/rfc/rfc9110#status.204", "204-null-or-empty");
        public static Error Missing(string code = "", string message = "") => new(ErrorType.Missing, code, message, 204, "https://www.rfc-editor.org/rfc/rfc9110#status.204", "204-missing");
        public static Error Bad(string code = "", string message = "") => new(ErrorType.Bad, code, message, 400, "https://www.rfc-editor.org/rfc/rfc9110#name-400-bad-request", "400-bad-request");
        public static Error Unauthorized(string code = "", string message = "") => new(ErrorType.Unauthorized, code, message, 401, "https://www.rfc-editor.org/rfc/rfc9110#name-401-unauthorized", "401-unauthorized");
        public static Error Forbidden(string code = "", string message = "") => new(ErrorType.Forbidden, code, message, 403, "https://www.rfc-editor.org/rfc/rfc9110#name-403-forbidden", "403-forbidden");
        public static Error NotFound(string code = "", string message = "") => new(ErrorType.NotFound, code, message, 404, "https://www.rfc-editor.org/rfc/rfc9110#name-404-not-found", "404-not-found");
        public static Error Conflict(string code = "", string message = "") => new(ErrorType.Conflict, code, message, 409, "https://www.rfc-editor.org/rfc/rfc9110#name-409-conflict", "409-conflict");
        public static Error Validation(string code = "", string message = "", IDictionary<string, string[]>? Errors = null) => new(ErrorType.Validation, code, message, 422, "https://www.rfc-editor.org/rfc/rfc9110#name-422-unprocessable-content", "422-unprocessable-content", "", Errors);
        public static Error RateLimit(string code = "", string message = "") => new(ErrorType.RateLimit, code, message, 429, "https://www.rfc-editor.org/rfc/rfc6585#section-4", "429-rate-limit");
        public static Error Server(string code = "", string message = "") => new(ErrorType.Server, code, message, 500, "https://www.rfc-editor.org/rfc/rfc9110#name-500-internal-server-error", "500-internal-server-error");
        public static Error Network(string code = "", string message = "") => new(ErrorType.Network, code, message, 502, "https://www.rfc-editor.org/rfc/rfc9110#name-502-bad-gateway", "502-bad-gateway"); 
    }
}
