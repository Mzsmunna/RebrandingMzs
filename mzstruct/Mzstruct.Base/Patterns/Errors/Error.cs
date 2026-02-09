using Mzstruct.Base.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Patterns.Errors
{
    public sealed record Error(
        ErrorType Type, 
        string Title, 
        string Message, 
        int StatusCode = 102, 
        string Url = "",
        Dictionary<string, string[]>? Details = null)
    {
        public static readonly Error None = new(ErrorType.None, string.Empty, string.Empty);
        
        public static Error Null(string title = "204-null-or-empty", string message = "Data for Requested query / payload seems missing", Dictionary<string, string[]>? details = null) => new(ErrorType.Null, title, message, 204, "https://www.rfc-editor.org/rfc/rfc9110#status.204", details);
        public static Error Missing(string title = "204-missing", string message = "Requested body payload seems empty or corrupted", Dictionary<string, string[]>? details = null) => new(ErrorType.Missing, title, message, 204, "https://www.rfc-editor.org/rfc/rfc9110#status.204", details);
        public static Error Bad(string title = "400-bad-request", string message = "We didn't recognize the user", Dictionary<string, string[]>? details = null) => new(ErrorType.Bad, title, message, 400, "https://www.rfc-editor.org/rfc/rfc9110#name-400-bad-request", details);
        public static Error Unauthorized(string title = "401-unauthorized", string message = "User may not have valid permission", Dictionary<string, string[]>? details = null) => new(ErrorType.Unauthorized, title, message, 401, "https://www.rfc-editor.org/rfc/rfc9110#name-401-unauthorized", details);
        public static Error Forbidden(string title = "403-forbidden", string message = "Data Not found", Dictionary<string, string[]>? details = null) => new(ErrorType.Forbidden, title, message, 403, "https://www.rfc-editor.org/rfc/rfc9110#name-403-forbidden", details);
        public static Error NotFound(string title = "404-not-found", string message = "Data Maybe already exists", Dictionary<string, string[]>? details = null) => new(ErrorType.NotFound, title, message, 404, "https://www.rfc-editor.org/rfc/rfc9110#name-404-not-found", details);
        public static Error Conflict(string title = "409-conflict", string message = "Data Maybe already reserved", Dictionary<string, string[]>? details = null) => new(ErrorType.Conflict, title, message, 409, "https://www.rfc-editor.org/rfc/rfc9110#name-409-conflict", details);
        public static Error Overflow(string title = "409-overflow", string message = "Data Maybe already booked / reserved", Dictionary<string, string[]>? details = null) => new(ErrorType.Overflow, title, message, 409, "https://www.rfc-editor.org/rfc/rfc9110#name-409-conflict", details);
        public static Error Validation(string title = "422-unprocessable-content", string message = "Payload seems invalid", Dictionary<string, string[]>? details = null) => new(ErrorType.Validation, title, message, 422, "https://www.rfc-editor.org/rfc/rfc9110#name-422-unprocessable-content", details);
        public static Error RateLimit(string title = "429-rate-limit", string message = "Too many Requests", Dictionary<string, string[]>? details = null) => new(ErrorType.RateLimit, title, message, 429, "https://www.rfc-editor.org/rfc/rfc6585#section-4", details);
        public static Error Server(string title = "500-internal-server-error", string message = "Something went wrong", Dictionary<string, string[]>? details = null) => new(ErrorType.Server, title, message + "; " + Environment.StackTrace, 500, "https://www.rfc-editor.org/rfc/rfc9110#name-500-internal-server-error", details);
        public static Error Network(string title = "502-bad-gateway", string message = "Network issue while fetching data", Dictionary<string, string[]>? details = null) => new(ErrorType.Network, title, message, 502, "https://www.rfc-editor.org/rfc/rfc9110#name-502-bad-gateway", details);
    }
}
