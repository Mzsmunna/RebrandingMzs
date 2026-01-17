using Mzstruct.Base.Entities;
using Mzstruct.Base.Enums;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Models;

public class Permission : BaseEntity
{
    public PermissionType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Roles { get; set; } = string.Empty; // "xyz,abc"
    public string? Regex { get; set; }
    public List<ResourceControl>? Sources { get; set; }
    public List<ConditionMap>? Conditions { get; set; }
    public string Permit { get; set; } = string.Empty; // "crueds" | "crud" | "cr" | "r" | "u" | "e" | "d" | "s" | etc.
    public string Restrict { get; set; } = string.Empty; // "crueds" | "crud" | "cr" | "r" | "u" | "e" | "d" | "s" | etc.
    public PrivacyType Privacy { get; set; } // system, platform, app, service, public, private, protected, custom, friend, friends_of_friend, followers, following, suscribers, subscribed, etc.
    public int Priority { get; set; }
    public bool IsActive { get; set; }
    public bool IsAllowedAny { get; set; } // top level permission
    public bool IsReadOnly { get; set; } // will override canCreate, canUpdate, canEdit, canDelete permissions
    public bool IsRestricted { get; set; } // will revoke all permissions
    public string? RestrictionId { get; set; }
    public IdentityMap? Permittor { get; set; } // system, platform, app, service, user, admin, manager, moderator, player, member etc.
    public DateTime? ExpiredAt { get; set; }
    public DateTime? RestrictedAt { get; set; }
}

public class PagePermission : Permission
{
    public List<string>? HtmlIds { get; set; } // selector
    public List<string>? HtmlClasses { get; set; } // selector
    public List<string>? HtmlTags { get; set; } // selector
    public List<string>? ApiUrls { get; set; }
    public List<PagePermission>? SubPages { get; set; }
}

//public class ApiPermission : Permission
//{
//    public List<ApiPermission>? SubApis { get; set; }
//}

public class ModulePermission : Permission
{
    public List<PagePermission>? Pages { get; set; }
    public List<ApiPermission>? Apis { get; set; }
    public string? Icon { get; set; }
    public string? Img { get; set; }
}

public class ResourcePermission : Permission
{
    public List<IdentityMap> Users { get; set; } = new List<IdentityMap>();
    public string ResourceId { get; set; } = string.Empty;
    public string ResourceType { get; set; } = string.Empty; // page, group, channel, chatroom, team, etc.
    public IdentityMap? ResourceCreater { get; set; }
    public IdentityMap? ResourceOwner { get; set; }
    public string? Icon { get; set; }
    public string? Img { get; set; }
}

public class ContentPermission : Permission
{
    public List<IdentityMap> Users { get; set; } = new List<IdentityMap>();
    public string ContentId { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty; // post, blog, thread, comment, document -> word | excel | pdf | text | slide, file -> image, video, audio, etc.
    public IdentityMap? ContentCreater { get; set; }
    public IdentityMap? ContentOwner { get; set; }
    public string ResourceId { get; set; } = string.Empty;
    public string ResourceType { get; set; } = string.Empty; // page, group, channel, chatroom, team, etc.
    public IdentityMap? ResourceCreater { get; set; }
    public IdentityMap? ResourceOwner { get; set; }
    public string? Icon { get; set; }
    public string? Img { get; set; }
}

public class PlatformPermission : Permission
{
    // UI permissions
    public List<ModulePermission>? Modules { get; set; }
    public List<PagePermission>? Pages { get; set; }
    // API permissions
    public List<ApiPermission>? Apis { get; set; }
}

//public class AppPermission : Permission
//{
//    // Platform permissions
//    public List<PlatformPermission>? Platforms { get; set; }
//}

//public class UserPermission : Permission
//{
//    public IdentityMap User { get; set; }
//    public bool IsPermissionExtended { get; set; }
//    public bool IsPermissionOverride { get; set; }
//    // App permissions
//    public List<AppPermission>? Apps { get; set; }
//    // Platform permissions
//    public List<PlatformPermission>? Platforms { get; set; }
//    // UI permissions
//    public List<ModulePermission>? Modules { get; set; }
//    public List<PagePermission>? Pages { get; set; }
//    // API permissions
//    public List<ApiPermission>? Apis { get; set; }
//}


