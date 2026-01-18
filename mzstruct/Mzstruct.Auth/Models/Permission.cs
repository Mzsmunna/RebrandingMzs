using Mzstruct.Base.Entities;
using Mzstruct.Base.Enums;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Mzstruct.Auth.Models;

public class Permission : BaseEntity
{
    public required PermissionType Type { get; set; }
    public required PrivacyType Privacy { get; set; }
    public string Permit { get; set; } = string.Empty; // AccessType: "crueds" | "crud" | "cr" | "r" | "u" | "e" | "d" | "s" | etc.
    public string Restrict { get; set; } = string.Empty; // AccessType: "crueds" | "crud" | "cr" | "r" | "u" | "e" | "d" | "s" | etc.
    public bool IsActive { get; set; } = false;
    public bool AllowListedOnly { get; set; } = false; // sub or child permission will inherit parent permission if missing
    public int Priority { get; set; }
    public string Version { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? PlatformId { get; set; }
    public string? AppId { get; set; }
    public string? AppSecret { get; set; }
    public string? AppTenant { get; set; }
    public string? Tags { get; set; } // "xyz,abc"
    public string? Icon { get; set; }
    public string? Img { get; set; }
    public List<ConditionMap>? Conditions { get; set; }
    public string? RestrictionId { get; set; } // will revoke all permissions
    public ReferenceMap? Permittor { get; set; } // system, platform, app, service, user, admin, manager, moderator, player, member etc.
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
    public List<FieldPermission>? FieldPermits { get; set; }
    
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
    public required string ResourceId { get; set; } = string.Empty;
    public required string ResourceType { get; set; } = string.Empty; // channel | chatroom | group | gang | page | shop | team | etc.
    public required ReferenceMap Creater { get; set; }
    public List<RolePermission>? RolePermits { get; set; }
    public ReferenceMap? Owner { get; set; }
}

public class ContentPermission : Permission
{
    public required string ContentId { get; set; } = string.Empty;
    public required string ContentType { get; set; } = string.Empty; // post, blog, thread, comment, document -> word | excel | pdf | text | slide, file -> image, video, audio, etc.
    public string? ParentContentId { get; set; }
    public string? ParentContentType { get; set; } // post, blog, thread, comment, document -> word | excel | pdf | text | slide, file -> image, video, audio, etc.
    public string? ResourceId { get; set; }
    public string? ResourceType { get; set; } // user | profile | channel | chatroom | group | gang | page | shop | team | etc.
    public ReferenceMap? ResourceOwner { get; set; }
    public ReferenceMap? Creater { get; set; }
}

public class PlatformPermission : Permission
{
    // App permissions
    public List<APPPermission>? AppPermits { get; set; }
}

public class APPPermission : Permission
{ 
    public string? PlatformPermitId { get; set; }   
    public List<ResourcePermission>? ResourcePermits { get; set; }
    public List<APIPermission>? ApiPermits { get; set; }
    public List<UIPermission>? UIPermits { get; set; } // Micro Frontend
    public List<ModulePermission>? ModulePermits { get; set; }
}

public class APIPermission : Permission
{
    public string? PlatformPermitId { get; set; }
    public string? AppPermitId { get; set; }
    public List<string>? Controllers { get; set; }
    public List<string>? EndPoints { get; set; }
    public List<ApiControllerPermission>? ControllerPermits { get; set; }
}

public class ApiControllerPermission : Permission 
{
    public string? PlatformPermitId { get; set; }
    public string? AppPermitId { get; set; }
    public string? ApiPermitId { get; set; }
    public List<string>? EndPoints { get; set; }
    public List<Permission>? EndPointPermits { get; set; }
}

//public class ApiEndPointPermission : Permission { }

public abstract class ViewPermission : Permission
{
    public string? PlatformPermitId { get; set; }
    public string? AppPermitId { get; set; }
    public string? HtmlIds { get; set; } // selectors " #id1, #id2"
    public string? HtmlClasses { get; set; } // selectors ".class1, .class2"
    public string? HtmlTags { get; set; } // selectors "div, span, p, a, img, button, input, form, table, tr, td, ul, li, etc."
    public List<string>? Apis { get; set; }
    public List<string>? Modules { get; set; }
    public List<string>? Components { get; set; }
    public List<string>? Sections { get; set; }
    public List<string>? Tabs { get; set; }
}

public class UIPermission : ViewPermission
{
    public List<ModulePermission>? ModulePermits { get; set; }
}

public class ModulePermission : ViewPermission
{
    public string? UIPermitId { get; set; }
    public List<PagePermission>? PagePermits { get; set; }
}

public class PagePermission : ViewPermission
{
    public string? UIPermitId { get; set; }
    public string? ModulePermitId { get; set; }
    public List<ComponentPermission>? ComponentPermits { get; set; }
}

public class ComponentPermission : ViewPermission
{
    public string? UIPermitId { get; set; }
    public string? ModulePermitId { get; set; }
    public string? PagePermitId { get; set; }
    // API permissions
    public List<APIPermission>? ApiPermits { get; set; }
}

public class RolePermission : Permission
{
    public string Roles { get; set; } = string.Empty; // "xyz,abc"
    public string? GroupPermitId { get; set; }
    public List<ResourcePermission>? ResourcePermits { get; set; }
    public List<APIPermission>? ApiPermits { get; set; }
    public List<ApiControllerPermission>? ControllerPermits { get; set; }
    public List<Permission>? EndPointPermits { get; set; }
    public List<ModulePermission>? ModulePermits { get; set; }
    public List<PagePermission>? PagePermits { get; set; }
    public List<ComponentPermission>? ComponentPermits { get; set; }
}

public class USERPermission : Permission
{
    public required ReferenceMap User { get; set; }
    public bool IsOverridden { get; set; } // prioritize this class over other classes (Permission tables / collections)
    public bool IsExtended { get; set; } // prioritize top level Permissions rather than the nested ones
    // Role Permissions
    public List<RolePermission>? RolePermits { get; set; }
    // Platform Permissions
    public List<PlatformPermission>? PlatformPermits { get; set; }
    // App Permissions
    public List<AppPermission>? AppPermits { get; set; }
    // API Permissions
    public List<APIPermission>? ApiPermits { get; set; }
    // UI Permissions
    public List<UIPermission>? UIPermits { get; set; } // Micro Frontend
    public List<ModulePermission>? ModulePermits { get; set; }
    public List<PagePermission>? PagePermits { get; set; }
    public List<ComponentPermission>? ComponentPermits { get; set; }
    // Feature Permissions
    public List<GroupPermission>? GroupPermits { get; set; }
    public List<ContentPermission>? ContentPermits { get; set; }
}