
using MongoDB.Bson.Serialization.Attributes;
using Mzstruct.Base.Entities;
using System;
using System.Collections.Generic;
using System.Security;

namespace Mzstruct.Auth.Models;

public class AuthPermission
{
    public string Id { get; set; } = string.Empty;
    //public string RoleNames { get; set; }
    //public List<string> PageUrls { get; set; }
    public List<AppPermission> AppPermissions { get; set; } = [];
    public int Order { get; set; }
    public bool IsPermissionAllowed { get; set; }
    public bool? IsReadOnlyAccess { get; set; }
    public bool IsActive { get; set; }
    public DateTime? PermissionExpiredOn { get; set; }
}

public class UserPermission : BaseEntity
{
    public string ClientId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    //[BsonIgnore]
    public string AssignedToken { get; set; } = string.Empty;
    public bool IsAnyApiAllowed { get; set; }
    public bool IsReadOnlyAccess { get; set; }
    public bool IgnoreAuthClaims { get; set; }
    public int ApiVersion { get; set; } = 0;
    public List<AppPermission> AppPermissions { get; set; } = [];
    public List<ApiPermission> ApiPermissions { get; set; } = [];
    public DateTime? ExpiredOn { get; set; }
    public bool IsActive { get; set; }
}

public class AppPermission
{
    public string Id { get; set; } = string.Empty;
    public string AppName { get; set; } = string.Empty;
    public string AppType { get; set; } = string.Empty;
    public List<ApiPermission> ApiPermissions { get; set; } = [];
    public bool IsAppAllowed { get; set; }
    public bool IsAnyApiAllowed { get; set; }
    public bool IsReadOnlyAccess { get; set; }
    public int ApiVersion { get; set; } = 0;
    public DateTime? ExpiredOn { get; set; }
    public bool IsActive { get; set; }
}

public class ApiPermission
{
    public string Controller { get; set; } = string.Empty;
    public string ControllerUrl { get; set; } = string.Empty;
    public string AppId { get; set; } = string.Empty;
    //public string RoleNames { get; set; }
    //public List<string> PageUrls { get; set; }
    public List<ConditionalPermission> ConditionalPermissions { get; set; } = [];
    public List<EndPointPermission> EndPointPermissions { get; set; } = [];
    public int ApiVersion { get; set; } = 0;
    public int Order { get; set; }
    public bool IsPermissionAllowed { get; set; }
    public bool? IsReadOnlyAccess { get; set; }
    public bool IsActive { get; set; }
    public DateTime? ExpiredOn { get; set; }
}

public class EndPointPermission
{
    public string AppId { get; set; } = string.Empty;
    public string EndPoint { get; set; } = string.Empty;
    public string EndPointUrl { get; set; } = string.Empty;
    //public string RoleNames { get; set; }
    //public List<string> PageUrls { get; set; }
    public List<ConditionalPermission> ConditionalPermissions { get; set; } = [];
    public int ApiVersion { get; set; } = 0;
    public int Order { get; set; }
    public bool IsPermissionAllowed { get; set; }
    public bool? IsReadOnlyAccess { get; set; }
    public bool IsActive { get; set; }
    public DateTime? ExpiredOn { get; set; }
}

public class ConditionalPermission
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string ValueType { get; set; } = string.Empty;
    public string LogicalOperator { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsAnd { get; set; }
    public bool IsCaseSensitive { get; set; }
    public bool IsActive { get; set; }
    public DateTime? ExpiredOn { get; set; }
}
