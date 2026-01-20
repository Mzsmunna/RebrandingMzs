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
    public bool CanUpload { get; set; } = false; // file | doc upload
    public int Priority { get; set; } = 0; // higher number means higher priority
    public string Version { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? ParentPermitId { get; set; }
    public string? Tags { get; set; } // "xyz,abc"
    public string? Icon { get; set; }
    public string? Img { get; set; }
    public string? RestrictionId { get; set; } // will revoke all permissions

    public SecretKey? PlatformSecret { get; set; }
    public SecretKey? AppSecret { get; set; }
    public List<ConditionMap>? Conditions { get; set; }    
    public ReferenceMap? Permittor { get; set; } // system, platform, app, service, user, admin, manager, moderator, player, member etc.
    public DateTime? DisabledAt { get; set; }
    public DateTime? RestrictedAt { get; set; }
}

public class FeaturePermission : Permission
{
    public List<string>? ResourcePermitIds { get; set; }
    public List<ResourcePermission>? ResourcePermits { get; set; }

    public List<string>? ApiPermitIds { get; set; }
    public List<APIPermission>? ApiPermits { get; set; }

    public List<string>? ControllerPermitIds { get; set; }
    public List<ApiControllerPermission>? ControllerPermits { get; set; }

    public List<string>? EndPointPermitIds { get; set; }
    public List<Permission>? EndPointPermits { get; set; }

    public List<string>? ModulePermitIds { get; set; }
    public List<ModulePermission>? ModulePermits { get; set; }

    public List<string>? PagePermitIds { get; set; }
    public List<PagePermission>? PagePermits { get; set; }

    public List<string>? ComponentPermitIds { get; set; }
    public List<ComponentPermission>? ComponentPermits { get; set; }
}

public class PaidFeaturePermission : FeaturePermission
{
    public string FeaturePermitId { get; set; } = string.Empty;
    public string MerchantId { get; set; } = string.Empty; // API Key
    public string PaymentId { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public string TransactionStatus { get; set; } = string.Empty;
    public string ApprovalCode { get; set; } = string.Empty;
    public string ReferenceNo { get; set; } = string.Empty; // Retrieval Reference Number (RRN)

    public string? OrderId { get; set; }
    public string? InvoiceId { get; set; }
    public string? EnrollId { get; set; }
    public string? MembershipId { get; set; }
    public string? SubscriptionId { get; set; }

    public DateTime ActivatedAt { get; set; }
    public DateTime DeactivatedAt { get; set; }
}

public class ResourcePermission : Permission
{
    ResourcePermission()
    {
        Type = PermissionType.Resource;
    }
    public required string Resources { get; set; } // collection or table names! | "xyz,abc"
    
    public List<string>? FieldPermitIds { get; set; }
    public List<FieldPermission>? FieldPermits { get; set; }
}

public class FieldPermission : Permission
{
    FieldPermission()
    {
        Type = PermissionType.Field;
    }
    public required string Resource { get; set; } // collection or table names! | "xyz,abc"
    public required string Property { get; set; } // collection field or table column or form field names!
}

public class ScopePermission : Permission
{
    public required string ResourceId { get; set; } = string.Empty;
    public required string ResourceType { get; set; } = string.Empty; // channel | chatroom | group | gang | page | shop | team | etc.
    public required ReferenceMap Creater { get; set; }
    public ReferenceMap? Owner { get; set; }

    public List<string>? RolePermitIds { get; set; }
    public List<ScopedRolePermission>? RolePermits { get; set; }
}

public class ContentPermission : Permission
{
    public required string ContentId { get; set; } = string.Empty;
    public required string ContentType { get; set; } = string.Empty; // post, blog, thread, comment, document -> word | excel | pdf | text | slide, file -> image, video, audio, etc.
    public string? ParentContentId { get; set; }
    public string? ParentContentType { get; set; } // post, blog, thread, comment, document -> word | excel | pdf | text | slide, file -> image, video, audio, etc.
    public string? ResourceId { get; set; }
    public string? ResourceType { get; set; } // user | profile | channel | chatroom | group | gang | page | shop | team | etc.
    public bool CanDownload { get; set; } = false; // file download
    public bool CanShare { get; set; } = false; // content share
    public bool CanComment { get; set; } = false; // content comment
    public bool CanLike { get; set; } = false; // content like
    public bool CanRate { get; set; } = false; // content rate
    public bool CanReport { get; set; } = false; // content report
    public bool CanBookmark { get; set; } = false; // content bookmark
    public bool CanStream { get; set; } = false; // content stream
    public ReferenceMap? ResourceOwner { get; set; }
    public ReferenceMap? Creater { get; set; }
}

public class PlatformPermission : Permission
{
    // App permissions
    public List<string>? AppPermitIds { get; set; }
    public List<APPPermission>? AppPermits { get; set; }
}

public class APPPermission : Permission
{ 
    public string? PlatformPermitId { get; set; }

    public List<string>? ResourcePermitIds { get; set; }
    public List<ResourcePermission>? ResourcePermits { get; set; }

    public List<string>? ApiPermitIds { get; set; }
    public List<APIPermission>? ApiPermits { get; set; }

    public List<string>? UIPermitIds { get; set; }
    public List<UIPermission>? UIPermits { get; set; } // Micro Frontend

    public List<string>? ModulePermitIds { get; set; }
    public List<ModulePermission>? ModulePermits { get; set; }
}

public class APIPermission : Permission
{
    public string? PlatformPermitId { get; set; }
    public string? AppPermitId { get; set; }

    public List<string>? Controllers { get; set; }
    public List<string>? EndPoints { get; set; }

    public List<string>? EndPointPermitIds { get; set; }
    public List<string>? ControllerPermitIds { get; set; }
    public List<ApiControllerPermission>? ControllerPermits { get; set; }
}

public class ApiControllerPermission : Permission 
{
    public string? PlatformPermitId { get; set; }
    public string? AppPermitId { get; set; }
    public string? ApiPermitId { get; set; }

    public List<string>? EndPoints { get; set; }
    public List<string>? EndPointPermitIds { get; set; }
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
    public List<string>? Controllers { get; set; }
    public List<string>? EndPoints { get; set; }
    public List<string>? Modules { get; set; }
    public List<string>? Pages { get; set; }
    public List<string>? Components { get; set; }
    public List<string>? Sections { get; set; }
    public List<string>? Tabs { get; set; }
    
    // API permissions
    public List<string>? ApiPermitIds { get; set; }
    public List<APIPermission>? ApiPermits { get; set; }

    public List<string>? ControllerPermitIds { get; set; }
    public List<ApiControllerPermission>? ControllerPermits { get; set; }

    public List<string>? EndPointPermitIds { get; set; }
    public List<Permission>? EndPointPermits { get; set; }
}

public class UIPermission : ViewPermission
{
    public List<string>? ModulePermitIds { get; set; }
    public List<ModulePermission>? ModulePermits { get; set; }
}

public class ModulePermission : ViewPermission
{
    public string? UIPermitId { get; set; }

    public List<string>? PagePermitIds { get; set; }
    public List<PagePermission>? PagePermits { get; set; }
}

public class PagePermission : ViewPermission
{
    public string? UIPermitId { get; set; }
    public string? ModulePermitId { get; set; }

    public List<string>? ComponentPermitIds { get; set; }
    public List<ComponentPermission>? ComponentPermits { get; set; }
}

public class ComponentPermission : ViewPermission
{
    public string? UIPermitId { get; set; }
    public string? ModulePermitId { get; set; }
    public string? PagePermitId { get; set; }

    public List<string>? SectionPermitIds { get; set; }
    public List<SectionPermission>? SectionPermits { get; set; }
}

public class SectionPermission : ViewPermission 
{
    public List<string>? TabPermitIds { get; set; }
    public List<Permission>? TabPermits { get; set; }
}

//public class TabPermission : ViewPermission { }

public class AppRolePermission : FeaturePermission
{
    public string Roles { get; set; } = string.Empty; // "xyz,abc"
}

public class ScopedRolePermission : AppRolePermission
{
    public required string GroupId { get; set; }
    public string? PermitId { get; set; } // ScopePermitId
}

public abstract class ConsumerPermission : Permission
{
    //public bool IsExtended { get; set; } // prioritize top level Permissions rather than the nested ones

    // Privileges
    public string Roles { get; set; } = string.Empty; // "xyz, abc"
    public DateTime ActivatedAt { get; set; }
    public DateTime? DeactivatedAt { get; set; }

    // Role Permissions
    public List<string>? RolePermitIds { get; set; }
    public List<AppRolePermission>? RolePermits { get; set; }
    
    // Platform Permissions
    public List<string>? PlatformIds { get; set; }
    public List<PlatformPermission>? PlatformPermits { get; set; }
    
    // App Permissions
    public List<string>? AppIds { get; set; }
    public List<AppPermission>? AppPermits { get; set; }
    
    // API Permissions
    public List<string>? ApiPermitIds { get; set; }
    public List<APIPermission>? ApiPermits { get; set; }
    
    // UI Permissions
    public List<string>? UIPermitIds { get; set; } // Micro Frontend
    public List<UIPermission>? UIPermits { get; set; } // Micro Frontend

    public List<string>? ModulePermitIds { get; set; }
    public List<ModulePermission>? ModulePermits { get; set; }

    public List<string>? PagePermitIds { get; set; }
    public List<PagePermission>? PagePermits { get; set; }

    public List<string>? ComponentPermitIds { get; set; }
    public List<ComponentPermission>? ComponentPermits { get; set; }

    public List<string>? SectionPermitIds { get; set; }
    public List<SectionPermission>? SectionPermits { get; set; }

    public List<string>? TabPermitIds { get; set; }
    public List<Permission>? TabPermits { get; set; }
    
    // Feature Permissions
    public List<string>? FeaturePermitIds { get; set; }
    public List<FeaturePermission>? FeaturePermits { get; set; }

    public List<string>? PaidFeaturePermitIds { get; set; }
    public List<PaidFeaturePermission>? PaidFeaturePermits { get; set; }

    public List<string>? ScopedRolePermitIds { get; set; }
    public List<ScopedRolePermission>? ScopedRolePermits { get; set; }

    public List<string>? ContentPermitIds { get; set; }
    public List<ContentPermission>? ContentPermits { get; set; }
}

public class USERPermission : ConsumerPermission
{
    public required ReferenceMap User { get; set; }
    public bool IsOverridden { get; set; } // prioritize this class over other classes (Permission tables / collections)
}

public class AdminPermission : ConsumerPermission
{
    public required ReferenceMap Admin { get; set; }
    public bool IsOverridden { get; set; } // prioritize this class over other classes (Permission tables / collections)
}

public class ClientPermission : ConsumerPermission
{
    public required ReferenceMap Client { get; set; }
    public SecretKey? ClientSecret { get; set; }
}