using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Models
{
    public class Field
    {
        public required string Key { get; set; }
        public string? Value { get; set; }
    }

    public class IconField : Field
    {
        public string KeyIcon { get; set; } = string.Empty;
        public string KeyLink { get; set; } = string.Empty;
        public string ValueIcon { get; set; } = string.Empty;
        public string ValueLink { get; set; } = string.Empty;
    }

    public class KeyMap : Field
    {
        public required string Name { get; set; }
        public string Label { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }

    public class ReferenceMap
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Img { get; set; }
        public int? No { get; set; }
    }

    public class EventMap : ReferenceMap
    {
        public DateTime At { get; set; }
        public string Type { get; set; } = string.Empty; // invite, request, accept, reject, remove, block, unblock, promote, demote, etc.
        public string? Details { get; set; }
        public string? Context { get; set; } // json encoded | base64 format for additional information
    }

    public class IdentityMap : ReferenceMap
    {
        public object? Role { get; set; } // string or FieldMap or null
        public object? Type { get; set; } // string or FieldMap or null
        public string? Identifiers { get; set; } // type / resource / json -> ex: "{ role: 'Captain, Batsman', suffix: 'Mzs' }"
        public string? Qrc { get; set; } // QR Code
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }

    public class ConditionMap : KeyMap
    {
        string Url { get; set; } = string.Empty;
        string ValueType { get; set; } = string.Empty; // string, number, boolean, date, time, datetime, etc.
        List<string>? Resources { get; set; } // any, jwt, session, cookie, localStorage, queryParams, requestBody, db, collection or table name, etc.
        string? Regex { get; set; }
        string Permit { get; set; } = string.Empty; // "crueds" | "crud" | "cr" | "r" | "u" | "e" | "d" | "s" | etc.
        string Restrict { get; set; } = string.Empty; // "crueds" | "crud" | "cr" | "r" | "u" | "e" | "d" | "s" | etc.
        string Privacy { get; set; } = string.Empty; // system, platform, app, service, public, private, protected, custom, friend, friends_of_friend, followers, following, suscribers, subscribed, etc.
        string LogicalOperator { get; set; } = string.Empty; // and, or, not, xor, nand, nor, xnor, ===, ==, !=, >, <, >=, <=, gt, lt, gte, lte, ne, eq, con, in, ext, sw, ew, isn, isnn, etc.
        bool IsAnd { get; set; }
        bool IsCaseSensitive { get; set; }
        bool IsNullable { get; set; }
        bool IsActive { get; set; }
        bool IsRestricted { get; set; } // will revoke all permissions
        string? RestrictionId { get; set; }
        List<int>? Versions { get; set; }
        int Order { get; set; }
        int Priority { get; set; }
        DateTime? ExpiredAt { get; set; }
        DateTime? RestrictedAt { get; set; }
    }
}
