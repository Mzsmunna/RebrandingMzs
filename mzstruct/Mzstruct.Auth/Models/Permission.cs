using Mzstruct.Base.Entities;
using Mzstruct.Base.Enums;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Models;

public abstract class Permission : BaseEntity
{
    public required PermissionType Type { get; set; }
    public required PrivacyType Privacy { get; set; }
    public string AppId { get; set; } = string.Empty;
    public string AppSecret { get; set; } = string.Empty;
    public string AppTenant { get; set; } = string.Empty;
    public string Permit { get; set; } = string.Empty; // AccessType: "crueds" | "crud" | "cr" | "r" | "u" | "e" | "d" | "s" | etc.
    public string Restrict { get; set; } = string.Empty; // AccessType: "crueds" | "crud" | "cr" | "r" | "u" | "e" | "d" | "s" | etc.
    public bool IsActive { get; set; } = false;
    public bool IsListedOnly { get; set; } = false; // sub or child permission will inherit parent permission
    public int Priority { get; set; }
    public string Version { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Tags { get; set; } = string.Empty; // "xyz,abc"
    public string? Icon { get; set; }
    public string? Img { get; set; }
    public List<ConditionMap>? Conditions { get; set; }
    public string? RestrictionId { get; set; } // will revoke all permissions
    //public ReferenceMap? Permittor { get; set; } // system, platform, app, service, user, admin, manager, moderator, player, member etc.
    public DateTime? ExpiredAt { get; set; }
    public DateTime? RestrictedAt { get; set; }
}

public class FeaturePermission
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
    public bool CanLiveStream { get; set; } // content stream live
    public bool IsReadOnly { get; set; } // will override canCreate, canUpdate, canEdit, canDelete permissions
    public string? RestrictionId { get; set; } // will revoke all permissions
    public List<ConditionMap>? Conditions { get; set; }
}

public class ResourcePermission : Permission
{
    ResourcePermission()
    {
        Type = PermissionType.Resource;
    }
    public required string Resources { get; set; } // collection or table names! | "xyz,abc"
    public List<FieldPermission>? Fields { get; set; }
    
}

public class FieldPermission : Permission
{
    FieldPermission()
    {
        Type = PermissionType.Field;
    }
    public required string Resources { get; set; } // collection or table names! | "xyz,abc"
    public required string Properties { get; set; } // collection or table or form field names!
}

public class GroupPermission : Permission
{
    public string ResourceId { get; set; } = string.Empty;
    public string ResourceType { get; set; } = string.Empty; // channel | chatroom | group | gang | page | shop | team | etc.
    public ReferenceMap? Creater { get; set; }
    public ReferenceMap? Owner { get; set; }
}

public class ContentPermission : Permission
{
    public List<ReferenceMap> Users { get; set; } = new();
    public string ContentId { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty; // post, blog, thread, comment, document -> word | excel | pdf | text | slide, file -> image, video, audio, etc.
    public IdentityMap? ContentCreater { get; set; }
    public IdentityMap? ContentOwner { get; set; }
    public FeaturePermission? Features { get; set; }
    public string ResourceId { get; set; } = string.Empty;
    public string ResourceType { get; set; } = string.Empty; // page, group, channel, chatroom, team, etc.
    public ReferenceMap? Creater { get; set; }
    public ReferenceMap? Owner { get; set; }
}

public class PlatformPermission : Permission
{
    // App permissions
    public List<AppPermission>? Apps { get; set; }
    // API permissions
    public List<ApiPermission>? Apis { get; set; }
    // UI permissions
    public List<ModulePermission>? Modules { get; set; }
    public List<PagePermission>? Pages { get; set; }
    
}

//public class AppPermission : Permission
//{
//    // API permissions
//    public List<ApiPermission>? Apis { get; set; }
//    // UI permissions
//    public List<ModulePermission>? Modules { get; set; }
//    public List<PagePermission>? Pages { get; set; }
//}

public class RolePermission : Permission
{
    public string Roles { get; set; } = string.Empty; // "xyz,abc"
    public List<ResourcePermission>? Resources { get; set; }
    public List<ModulePermission>? Modules { get; set; }
    public List<PagePermission>? Pages { get; set; }
    public List<APIPermission>? Apis { get; set; }   
}

public class APIPermission : Permission
{
    public required string Controllers { get; set; }
    public required string EndPoints { get; set; }
    public List<APIPermission>? SubApis { get; set; }
}

public class ModulePermission : Permission
{
    public List<PagePermission>? Pages { get; set; }
    public List<APIPermission>? Apis { get; set; }   
}

public class PagePermission : Permission
{
    public string? HtmlIds { get; set; } // selectors " #id1, #id2"
    public string? HtmlClasses { get; set; } // selectors ".class1, .class2"
    public string? HtmlTags { get; set; } // selectors "div, span, p, a, img, button, input, form, table, tr, td, ul, li, etc."
    public List<string>? ApiUrls { get; set; }
    public List<PagePermission>? SubPages { get; set; }
}

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


