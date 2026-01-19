using Mzstruct.Base.Entities;
using Mzstruct.Base.Enums;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;

public class Restriction : BaseEntity
{
    public required ReferenceMap Restricted { get; set; }
    public required ReferenceMap Restrictor { get; set; }
    public Field? Resouce { get; set; } // collection | table name -> pageid, groupid, channelid, chatroomid, etc.
    public string Url { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public PermissionType Type { get; set; } // system, platform, app, user, module, api, page, resource, content, etc.
    public PrivacyType Privacy { get; set; }
    public string Restrict { get; set; } = string.Empty; // AccessType: "crueds" | "crud" | "cr" | "r" | "u" | "e" | "d" | "s" | etc. => c = create, r = read, u = update, e = edit, d = delete, s = search
    public string Reason { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Violation? Violation { get; set; }
    public DateTime? ActivatedAt { get; set; }
    public DateTime? DisabledAt { get; set; }
    public DateTime? DeactivatedAt { get; set; }
    public DateTime? RestrictedAt { get; set; }
}

public class Violation
{
    public required ReferenceMap Violator { get; set; } // user
    public ViolationType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    //public string? RestrictionId { get; set; }
    public List<string>? ReportIds { get; set; }
    public List<string>? Topics { get; set; }
    public List<string>? Censorships { get; set; }
    public List<string>? Rules { get; set; }
    public List<string>? Rights { get; set; }
    public List<string>? Policies { get; set; }
    public List<string>? Allegations { get; set; }
    public List<string>? Charges { get; set; }
    public List<string>? Evidences { get; set; }
}