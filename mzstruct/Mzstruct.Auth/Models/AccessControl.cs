using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Models;

public class AccessControl
{

}

public class ResourceControl
{
    public string Resources { get; set; } = string.Empty; // collection or table names! | "xyz,abc"
    public string Permit { get; set; } = string.Empty; // "crueds" | "crud" | "cr" | "r" | "u" | "e" | "d" | "s" | etc.
    public string Restrict { get; set; } = string.Empty; // "crueds" | "crud" | "cr" | "r" | "u" | "e" | "d" | "s" | etc.
    public string Privacy { get; set; } = string.Empty; // system, platform, app, service, public, private, protected, custom, friend, friends_of_friend, followers, following, suscribers, subscribed, etc.
    public List<FeatureControl>? Props { get; set; }
    public bool IsActive { get; set; }
    public bool IsRestricted { get; set; } // will revoke all permissions
    public string? RestrictionId { get; set; }
    public DateTime? RestrictedOn { get; set; }
}

public class FieldControl
{
    public string Properties { get; set; } // collection or table or form field names!
    public string Permit { get; set; } // "crueds" | "crud" | "cr" | "r" | "u" | "e" | "d" | "s" | etc.
    public string Restrict { get; set; } // "crueds" | "crud" | "cr" | "r" | "u" | "e" | "d" | "s" | etc.
    public string Privacy { get; set; } // system, platform, app, service, public, private, protected, custom, friend, friends_of_friend, followers, following, suscribers, subscribed, etc.
    public List<FieldControl>? SubFields { get; set; }
    public bool IsActive { get; set; }
    public bool IsRestricted { get; set; } // will revoke all permissions
    public string? RestrictionId { get; set; }
    public DateTime? RestrictedOn { get; set; }
}

public class FeatureControl
{
    public bool CanUpload { get; set; } // file upload
    public bool CanDownload { get; set; } // file download
    public bool CanPost { get; set; } // content post
    public bool CanShare { get; set; } // content share
    public bool CanComment { get; set; } // content comment
    public bool CanLike { get; set; } // content like
    public bool CanRate { get; set; } // content rate
    public bool CanReport { get; set; } // content report
    public bool CanBookmark { get; set; } // content bookmark
    public bool CanStream { get; set; } // content stream
    public bool CanDownloadStream { get; set; } // content stream download
    public bool CanShareStream { get; set; } // content stream share
    public bool CanLivestream { get; set; } // content stream live
    public List<ConditionMap>? Conditions { get; set; }
}

// Assuming ConditionMap is defined somewhere else in your codebase.
