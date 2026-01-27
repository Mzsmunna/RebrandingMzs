using Mzstruct.Base.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Errors
{
    public sealed record Error(ErrorType Type, string Code, string Details, int StatusCore = 102, string Url = "", string Title = "", IDictionary<string, string[]>? Messages = null)
    {
        public static readonly Error None = new(ErrorType.None, string.Empty, string.Empty);
        
        public static Error Null(string code = "", string details = "", IDictionary<string, string[]>? messages = null) => new(ErrorType.Null, code, details, 204, "https://www.rfc-editor.org/rfc/rfc9110#status.204", "204-null-or-empty", messages);
        public static Error Missing(string code = "", string details = "", IDictionary<string, string[]>? messages = null) => new(ErrorType.Missing, code, details, 204, "https://www.rfc-editor.org/rfc/rfc9110#status.204", "204-missing", messages);
        public static Error Bad(string code = "", string details = "", IDictionary<string, string[]>? messages = null) => new(ErrorType.Bad, code, details, 400, "https://www.rfc-editor.org/rfc/rfc9110#name-400-bad-request", "400-bad-request", messages);
        public static Error Unauthorized(string code = "", string details = "", IDictionary<string, string[]>? messages = null) => new(ErrorType.Unauthorized, code, details, 401, "https://www.rfc-editor.org/rfc/rfc9110#name-401-unauthorized", "401-unauthorized", messages);
        public static Error Forbidden(string code = "", string details = "", IDictionary<string, string[]>? messages = null) => new(ErrorType.Forbidden, code, details, 403, "https://www.rfc-editor.org/rfc/rfc9110#name-403-forbidden", "403-forbidden", messages);
        public static Error NotFound(string code = "", string details = "", IDictionary<string, string[]>? messages = null) => new(ErrorType.NotFound, code, details, 404, "https://www.rfc-editor.org/rfc/rfc9110#name-404-not-found", "404-not-found", messages);
        public static Error Conflict(string code = "", string details = "", IDictionary<string, string[]>? messages = null) => new(ErrorType.Conflict, code, details, 409, "https://www.rfc-editor.org/rfc/rfc9110#name-409-conflict", "409-conflict", messages);
        public static Error Validation(string code = "", string details = "", IDictionary<string, string[]>? messages = null) => new(ErrorType.Validation, code, details, 422, "https://www.rfc-editor.org/rfc/rfc9110#name-422-unprocessable-content", "422-unprocessable-content", messages);
        public static Error RateLimit(string code = "", string details = "", IDictionary<string, string[]>? messages = null) => new(ErrorType.RateLimit, code, details, 429, "https://www.rfc-editor.org/rfc/rfc6585#section-4", "429-rate-limit", messages);
        public static Error Server(string code = "", string details = "", IDictionary<string, string[]>? messages = null) => new(ErrorType.Server, code, details + "; " + Environment.StackTrace, 500, "https://www.rfc-editor.org/rfc/rfc9110#name-500-internal-server-error", "500-internal-server-error", messages);
        public static Error Network(string code = "", string details = "", IDictionary<string, string[]>? messages = null) => new(ErrorType.Network, code, details, 502, "https://www.rfc-editor.org/rfc/rfc9110#name-502-bad-gateway", "502-bad-gateway", messages);
    }
}
