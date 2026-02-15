using Mzstruct.Base.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Models
{
    public class FieldMap
    {
        public required string Name { get; set; }        
        public string Value { get; set; } = string.Empty;
    }

    public class KeyMap : FieldMap
    {
        public required string Key { get; set; }
        public string Label { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }

    public class SecretKey
    {
        public required string Id { get; set; }
        public string? Secret { get; set; }
        public string? Tenant { get; set; }
    }

    public class ReferenceMap
    {
        public required string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Img { get; set; }
        public int? No { get; set; }
        public string? Qrc { get; set; } // QR Code
        public string? Url { get; set; }
    }

    public class IdentityMap : ReferenceMap
    {
        public string Role { get; set; } = string.Empty; // string or FieldMap or null
        public string Group { get; set; } = string.Empty; // string or FieldMap or null
        public string? Tag { get; set; } = string.Empty; // "xyz,abc"
        public string? Identifier { get; set; } // type / resource / json -> ex: "{ role: 'Captain, Batsman', suffix: 'Mzs' }"
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Img { get; set; }
        public string? Cvr { get; set; }
    }

    public class EventMap : ReferenceMap
    {
        public DateTime TriggeredAt { get; set; }
        public string Type { get; set; } = string.Empty; // invite, request, accept, reject, remove, block, unblock, promote, demote, etc.
        public string? Details { get; set; }
        public string? Context { get; set; } // json encoded | base64 format for additional information
    }

    public class ConditionMap : FieldMap
    {
        public required string FieldType { get; set; } = string.Empty; // string, number, boolean, date, time, datetime, etc.
        public required string Checks { get; set; } // ContextType: any, this, jwt, session, cookie, localStorage, queryParams, requestBody, db, collection or table name, etc.
        public int Priority { get; set; } = 0;
        public bool IsAnd { get; set; }
        public bool IsCaseSensitive { get; set; }
        public bool IsNullable { get; set; }
        public bool IsActive { get; set; }
        public LogicalOperator? LogicalOperator { get; set; } // and, or, not, xor, nand, nor, xnor, ===, ==, !=, >, <, >=, <=, gt, lt, gte, lte, ne, eq, con, in, ext, sw, ew, isn, isnn, etc.
        public string? Permit { get; set; } = string.Empty; // AccessType: "crueds" | "crud" | "cr" | "r" | "u" | "e" | "d" | "s" | etc.
        public string? Restrict { get; set; } = string.Empty; // AccessType: "crueds" | "crud" | "cr" | "r" | "u" | "e" | "d" | "s" | etc.
        
        public string? Regex { get; set; }
        public string? RestrictionId { get; set; }
        public string? Versions { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public DateTime? RestrictedAt { get; set; }
    }
}
