using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Mzstruct.Auth.Contracts.IHandlers;
using Mzstruct.Auth.Helpers;
using Mzstruct.Auth.Models;
using Mzstruct.Base.Consts;
using Mzstruct.Base.Enums;
using Mzstruct.Cache.Contracts;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Mzstruct.Auth.Policies
{
    public class CustomAuthorizer : ICustomAuthorizer
    {
        public readonly IInMemoryCacher _inMemoryCacher;
        private readonly SemaphoreSlim concurrentRequestLocker;

        public CustomAuthorizer(IInMemoryCacher inMemoryCacher)
        {
            _inMemoryCacher = inMemoryCacher;
            concurrentRequestLocker = new SemaphoreSlim(1, 1);
        }

        //[Time("Dynamic Authorize Validation for {requestedUrl}")]
        public async Task<bool> ValidatePermissions(AuthorizationHandlerContext context, HttpContext httpContext, string requestedUrl)
        {
            bool isPermissionValidated = false;

            if (context != null && context.User != null && httpContext != null)
            {
                if (!context.User.HasClaim(x => x.Type == "RoleName"))
                    return true;

                if (string.IsNullOrEmpty(AppConst.ClientId) || !context.User.HasClaim(x => x.Type == "ClientId"))
                    isPermissionValidated = true;
                else if (!string.IsNullOrEmpty(AppConst.ClientId) &&
                    context.User.HasClaim(x => x.Type == "ClientId") &&
                    context.User.FindFirst(x => x.Type == "ClientId")?.Value == AppConst.ClientId)
                    isPermissionValidated = true;
                else
                    return false;

                if (context.User.HasClaim(x => x.Type == "AuthClaims"))
                {
                    isPermissionValidated = await ValidateApiClaims(context, httpContext, isPermissionValidated);
                }
                else
                {
                    UserPermission userAuthorizeConfig = await GetUserAuthorizeConfig(context, httpContext);
                    
                    if (userAuthorizeConfig != null)
                    {
                        if (context.User.HasClaim(x => x.Type == "ClientId") &&
                            context.User.FindFirst(x => x.Type == "ClientId")?.Value == userAuthorizeConfig.ClientId)
                            isPermissionValidated = await VerifyAccess(context, httpContext, userAuthorizeConfig, isPermissionValidated);
                        else
                            isPermissionValidated = false;
                    }
                    else
                        isPermissionValidated = true;
                }

            }

            return isPermissionValidated;
        }

        private async Task<UserPermission?> GetUserAuthorizeConfig(AuthorizationHandlerContext context, HttpContext httpContext)
        {
            var accessToken = await httpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token");
            UserPermission? userAuthorizeConfig = null;

            //if (context.User.HasClaim(x => x.Type == "UserAuthorization"))
            //{
            //    var jsonUserAuthorization = context.User.FindFirst(x => x.Type == "UserAuthorization")?.Value;
            //    if (!string.IsNullOrEmpty(jsonUserAuthorization))
            //        userAuthorizeConfig = JsonConvert.DeserializeObject<UserPermission>(jsonUserAuthorization);
            //}
            //else 
            if (!string.IsNullOrEmpty(accessToken))
            {
                var userId = JwtHelper.GetValueFromToken(accessToken, "UserId");
                var clientId = JwtHelper.GetValueFromToken(accessToken, "ClientId");
                //var userAuthorizeId = JwtHelper.GetValueFromToken(accessToken, "AuthClaims");
                var roleName = JwtHelper.GetValueFromToken(accessToken, "RoleName");

                if (!string.IsNullOrEmpty(userId))
                {
                    userAuthorizeConfig = _inMemoryCacher.GetData<UserPermission>(userId);

                    if (userAuthorizeConfig != null)
                    {
                        //!userAuthorizeConfig.AssignedToken.Equals(accessToken)
                        if (!userAuthorizeConfig.RoleName.Equals(roleName)) //|| !userAuthorizeConfig.ClientId.Equals(clientId))
                        {
                            if (_inMemoryCacher.RemoveData<UserPermission>(userId))
                                userAuthorizeConfig = null;
                        }
                    }
                    
                    if (userAuthorizeConfig == null)
                    {
                        await concurrentRequestLocker.WaitAsync();

                        try
                        {
                            userAuthorizeConfig = _inMemoryCacher.GetData<UserPermission>(userId);

                            //!userAuthorizeConfig.AssignedToken.Equals(accessToken)
                            if (userAuthorizeConfig != null && !userAuthorizeConfig.RoleName.Equals(roleName)) //|| !userAuthorizeConfig.ClientId.Equals(clientId)))
                            {
                                if (_inMemoryCacher.RemoveData<UserPermission>(userId))
                                    userAuthorizeConfig = null;
                            }

                            if (userAuthorizeConfig != null)
                            {
                                userAuthorizeConfig.AssignedToken = accessToken;
                                userAuthorizeConfig = _inMemoryCacher.SetData(userId, userAuthorizeConfig, DateTimeOffset.UtcNow.AddHours(1));
                            }
                        }
                        finally
                        {
                            concurrentRequestLocker.Release();
                        }                          
                    }
                }
            }

            return userAuthorizeConfig;
        }

        private async Task<IDictionary<string, object>> GetRequestBodyObject(AuthorizationHandlerContext context, HttpContext httpContext)
        {
            var requestBody = string.Empty;
            IDictionary<string, object> requestObject = null;

            if (httpContext.Request.Method.ToLower() == "get")
            {
                var requestQueries = HttpUtility.ParseQueryString(httpContext.Request.QueryString.Value);
                requestObject = requestQueries.AllKeys.ToDictionary(k => k, k => requestQueries[k] as object);
            }
            else
            {
                httpContext.Request.EnableBuffering();
                using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    requestBody = await reader.ReadToEndAsync();
                }
                httpContext.Request.Body.Position = 0;
                requestObject = JsonConvert.DeserializeObject<IDictionary<string, object>>(requestBody);

                var requestQueries = HttpUtility.ParseQueryString(httpContext.Request.QueryString.Value);
                var requestQueryObject = requestQueries.AllKeys.ToDictionary(k => k, k => requestQueries[k] as object);

                if (requestQueryObject != null && requestQueryObject.Count > 0)
                {
                    foreach ( var queryParam in requestQueryObject )
                    {
                        if (!requestObject.ContainsKey(queryParam.Key))
                            requestObject.Add(queryParam);
                    }
                }
            }

            if (context.User.Claims != null && context.User.Claims.Count() > 0)
            {
                //var userClaims = context.User.Claims.Where(x => !x.Type.Contains("/api/"))
                //    .Select(claim => new { claim.Type, claim.Value })
                //    .ToDictionary(k => k.Type, k => k.Value as object);

                foreach (var claim in context.User.Claims)
                {
                    if (claim.Type.Contains("/api/") || 
                        requestObject.ContainsKey(claim.Type))
                        continue;

                    var duplicateClaims = context.User.Claims.Where(x => x.Type.Equals(claim.Type)).ToList();
                    var value = string.Empty;
                    var multiValues = new List<string>();

                    if (duplicateClaims.Count > 1)
                    {
                        multiValues = duplicateClaims.Select(x => x.Value).ToList();
                        value = string.Join(",", multiValues);
                    }
                    else
                        value = claim.Value;

                    if (!requestObject.ContainsKey(claim.Type))
                        requestObject.Add(claim.Type, value);
                }
            }

            return requestObject;
        }

        private async Task<bool> ValidateApiClaims(AuthorizationHandlerContext context, HttpContext httpContext, bool isPermissionValidated)
        {
            if (httpContext != null && context != null && context.User.HasClaim(x => x.Type == "AuthClaims"))
            {
                var endPointUrl = httpContext.Request.Path.ToString().ToLower();
                var controllerUrl = endPointUrl.Remove(endPointUrl.LastIndexOf('/'));
                var endPoint = endPointUrl.Split("/").LastOrDefault();

                List<Claim> requestedControllerClaims = context.User.Claims.Where(x => x.Type.Contains(controllerUrl)).ToList();
                Claim controllerClaim = null;
                Claim endPointClaim = null;
                AppPermission appPermission = null;
                
                if (!string.IsNullOrEmpty(AppConst.AppId))
                {
                    var appClaim = context.User.Claims.Where(x => x.Type.Contains($"{AppConst.AppId.ToLower()}")).FirstOrDefault();
                    int apiVersion = 0;
                    var versionPattern = @"v(\d+)";
                    Match apiVersionMatch = null;

                    if (appClaim != null)
                    {
                        appPermission = new AppPermission();
                        appPermission.Id = AppConst.AppId.ToLower();
                        apiVersionMatch = Regex.Match(appClaim.Type, versionPattern);
                        if (apiVersionMatch != null && apiVersionMatch.Success)
                            appPermission.ApiVersion = int.TryParse(apiVersionMatch.Value.Replace("v", ""), out apiVersion) ? apiVersion : 0;
                        if (!string.IsNullOrEmpty(appClaim.Value) &&
                            appClaim.Value.ToLower().Equals("deny"))
                            appPermission.IsAppAllowed = false;
                    }

                    if (appPermission == null)
                    {
                        appClaim = context.User.Claims.Where(x => x.Type.Equals("AllowAnyApi") && x.Value.Contains($"{AppConst.AppId.ToLower()}")).FirstOrDefault();

                        if (appClaim != null)
                        {
                            appPermission = new AppPermission();
                            appPermission.Id = AppConst.AppId.ToLower();                          
                            appPermission.IsAppAllowed = true;
                            appPermission.IsAnyApiAllowed = true;
                            appPermission.IsReadOnlyAccess = false;
                            apiVersionMatch = Regex.Match(appClaim.Value, versionPattern);
                            if (apiVersionMatch != null && apiVersionMatch.Success)
                                appPermission.ApiVersion = int.TryParse(apiVersionMatch.Value.Replace("v", ""), out apiVersion) ? apiVersion : 0;
                        }
                    }

                    if (appPermission == null)
                    {
                        appClaim = context.User.Claims.Where(x => x.Type.Equals("ReadOnlyApi") && x.Value.Contains($"{AppConst.AppId.ToLower()}")).FirstOrDefault();

                        if (appClaim != null)
                        {
                            appPermission = new AppPermission();
                            appPermission.Id = AppConst.AppId.ToLower();
                            appPermission.IsAppAllowed = true;
                            appPermission.IsAnyApiAllowed = true;
                            appPermission.IsReadOnlyAccess = true;
                            apiVersionMatch = Regex.Match(appClaim.Value, versionPattern);
                            if (apiVersionMatch != null && apiVersionMatch.Success)
                                appPermission.ApiVersion = int.TryParse(apiVersionMatch.Value.Replace("v", ""), out apiVersion) ? apiVersion : 0;
                        }
                    }

                    if (appPermission != null && !appPermission.IsAppAllowed)
                        return false;
                }

                if (requestedControllerClaims != null && requestedControllerClaims.Count > 0)
                {
                    var controllerClaimPermission = string.Empty;

                    if (requestedControllerClaims.Any(x => x.Type.Contains(":/")))
                    {
                        if (!string.IsNullOrEmpty(AppConst.AppId))
                        {
                            if (requestedControllerClaims.Any(x => x.Type.Contains($"{AppConst.AppId}:/")))
                                requestedControllerClaims = requestedControllerClaims.Where(x => x.Type.Contains($"{AppConst.AppId}:/")).ToList();
                            else
                                requestedControllerClaims = requestedControllerClaims.Where(x => !x.Type.Contains(":/")).ToList();
                        }
                        else
                            requestedControllerClaims = requestedControllerClaims.Where(x => !x.Type.Contains(":/")).ToList();
                    }

                    if (requestedControllerClaims != null && requestedControllerClaims.Count > 0)
                    {
                        controllerClaimPermission = requestedControllerClaims.FirstOrDefault().Value;
                        if (controllerClaimPermission.StartsWith("allow") ||
                            controllerClaimPermission.StartsWith("readonly") ||
                            controllerClaimPermission.StartsWith("deny"))
                        {
                            controllerClaim = requestedControllerClaims.FirstOrDefault();
                        }
                        endPointClaim = requestedControllerClaims.Where(x => x.Value.Contains(endPoint)).FirstOrDefault();
                    }                   
                }

                if (controllerClaim == null && endPointClaim == null)
                {
                    if (appPermission != null && appPermission.ApiVersion > 0)
                    {
                        if (endPointUrl.StartsWith("/api/v"))
                        {
                            if (!endPointUrl.StartsWith($"/api/v{appPermission.ApiVersion}"))
                                return false;
                        }
                        else
                            return false;
                    }
                    else if (context.User.HasClaim(x => x.Type == "ApiVersion"))
                    {
                        int apiVersion = int.TryParse(context.User.FindFirst(x => x.Type == "ApiVersion")?.Value, out apiVersion) ? apiVersion : 0;
                        if (!endPointUrl.StartsWith($"/api/v{apiVersion}"))
                            return false;
                    }

                    if (appPermission != null)
                    {
                        if (appPermission.IsAppAllowed &&
                            appPermission.IsAnyApiAllowed && appPermission.IsReadOnlyAccess)
                        {
                            if (httpContext.Request.Method.ToLower().Equals("get"))
                                isPermissionValidated = true;
                            else if (endPoint.StartsWith("get"))
                                isPermissionValidated = true;
                            else
                                isPermissionValidated = false;
                        }
                        else if (appPermission.IsAppAllowed && 
                            appPermission.IsAnyApiAllowed && !appPermission.IsReadOnlyAccess)
                        {
                            isPermissionValidated = true;
                        }
                    }
                    else
                    {
                        if (context.User.Claims.Any(x => x.Type.Equals("ReadOnlyApi") && 
                            x.Value.Equals("")))
                        {
                            if (httpContext.Request.Method.ToLower().Equals("get"))
                                isPermissionValidated = true;
                            else if (endPoint.StartsWith("get"))
                                isPermissionValidated = true;
                            else
                                isPermissionValidated = false;
                        }
                        else if (context.User.Claims.Any(x => x.Type.Equals("AllowAnyApi") && 
                            x.Value.Equals("")))
                            isPermissionValidated = true;
                        else
                            isPermissionValidated = false;
                    }
                }
                else
                {
                    isPermissionValidated = await VerifyApiClaims(context, httpContext, controllerClaim, isPermissionValidated);
                    isPermissionValidated = await VerifyApiClaims(context, httpContext, endPointClaim, isPermissionValidated);
                }
            }

            return isPermissionValidated;
        }

        private async Task<bool> VerifyApiClaims(AuthorizationHandlerContext context, HttpContext httpContext, Claim apiClaim, bool isPermissionValidated)
        {
            if (httpContext != null && context != null && apiClaim != null)
            {
                var requestedMethod = httpContext.Request.Method.ToLower();
                var requestedUrl = httpContext.Request.Path.ToString().ToLower();
                var endPoint = requestedUrl.Split("/").LastOrDefault();
                var claimValue = apiClaim.Value;
                var versionPattern = @"v\d+";
                var claimApiVersion = Regex.Match(claimValue, versionPattern);

                if (string.IsNullOrEmpty(endPoint)) return false;
              
                if (claimValue.StartsWith("/v"))
                {
                    if (requestedUrl.StartsWith("/api/v"))
                    {
                        if (claimApiVersion.Success)
                        {
                            if (!requestedUrl.StartsWith($"/api/v{claimApiVersion.Value}"))
                                return false;
                        }
                    }
                    else
                        return false;
                    claimValue = claimValue.Replace($"/{claimApiVersion.Value}/", "");
                }
                else if (apiClaim.Type.Contains("/api/v"))
                {
                    if (requestedUrl.StartsWith("/api/v"))
                    {
                        if (claimApiVersion.Success)
                        {
                            if (!requestedUrl.StartsWith($"/api/v{claimApiVersion.Value}"))
                                return false;
                        }
                    }
                    else
                        return false;
                }

                if (claimValue.Contains($"{endPoint}:allow") || claimValue.Contains($"{endPoint}:readonly"))
                {
                    string claimValuePattern = @"^[^:]*:";
                    claimValue = Regex.Replace(claimValue, claimValuePattern, "");
                }

                if (claimValue == "allow")
                {
                    isPermissionValidated = true;
                }
                else if (claimValue == "readonly")
                {
                    if (requestedMethod.Equals("get"))
                        isPermissionValidated = true;
                    else if (endPoint.StartsWith("get"))
                        isPermissionValidated = true;
                    else
                        isPermissionValidated = false;
                }
                else if (claimValue == "deny")
                {
                    isPermissionValidated = false;
                }
                else if (claimValue.StartsWith("allowIf") || claimValue.StartsWith("readonlyIf"))
                {
                    bool isReadOnly = claimValue.StartsWith("readonlyIf") ? true : false;
                    claimValue = claimValue.Replace("allowIf?", "").Replace("readonlyIf?", "");
                    var conditions = claimValue.Split("^").ToList();
                    List<ConditionalPermission> conditionalPermissions = new List<ConditionalPermission>();

                    foreach (var condition in conditions)
                    {
                        string con = condition;
                        bool isAnd = con.StartsWith("&");
                        con = con.Replace("&", "").Replace("|", "");
                        var logic = con.Split( new string[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);

                        if (logic != null && logic.Length >= 3)
                        {
                            var conditionalObject = new ConditionalPermission();
                            conditionalObject.Key = logic[0];
                            conditionalObject.LogicalOperator = logic[1];
                            conditionalObject.Value = logic[2];
                            conditionalObject.ValueType = "string";
                            conditionalObject.IsAnd = isAnd;
                            conditionalObject.IsActive = true;
                            conditionalObject.IsCaseSensitive = false;
                            conditionalPermissions.Add(conditionalObject);
                        }
                    }

                    isPermissionValidated = await VerifyConditionalApiAccess(context, httpContext, conditionalPermissions, isPermissionValidated);

                    if (isPermissionValidated && isReadOnly)
                    {
                        if (requestedMethod.Equals("get"))
                            isPermissionValidated = true;
                        else if (endPoint.StartsWith("get"))
                            isPermissionValidated = true;
                        else
                            isPermissionValidated = false;
                    }
                }
            }

            return isPermissionValidated;
        }

        private async Task<bool> VerifyAccess(AuthorizationHandlerContext context, HttpContext httpContext, UserPermission userAuthorizeConfig, bool isPermissionValidated)
        {
            if (context != null && httpContext != null && userAuthorizeConfig != null && 
                userAuthorizeConfig.IsActive && userAuthorizeConfig.IgnoreAuthClaims)
            {
                var requestedHost = httpContext.Request.Host.ToString().ToLower();             
                var requestedUrl = httpContext.Request.Path.ToString().ToLower();
                var requestedMethod = httpContext.Request.Method.ToLower();
                var endPoint = requestedUrl.Split("/").LastOrDefault();
                string requestedUrlWithoutApiVersion = string.Empty;
                string appId = AppConst.AppId ?? string.Empty;

                AppPermission appConfig = null;
                ApiPermission controllerConfig = null;
                EndPointPermission endPointConfig = null;

                if (!string.IsNullOrEmpty(appId) &&
                    userAuthorizeConfig.AppPermissions != null && 
                    userAuthorizeConfig.AppPermissions.Count > 0)
                {
                    appConfig = userAuthorizeConfig.AppPermissions.Find(x =>
                        x.IsActive &&
                        !string.IsNullOrEmpty(x.Id) && 
                        appId.ToLower().Equals(x.Id.ToLower())
                    );

                    if (appConfig != null)
                    {
                        if (!appConfig.IsAppAllowed)
                            return false;
                        else if (appConfig.ExpiredOn.HasValue && 
                            appConfig.ExpiredOn.Value < DateTime.UtcNow)
                            return false;
                    }
                }

                if (userAuthorizeConfig.ExpiredOn.HasValue && 
                    userAuthorizeConfig.ExpiredOn.Value < DateTime.UtcNow)
                            return false;

                if (requestedUrl.StartsWith("/api/v"))
                {
                    //string pattern = @"^/api(/v\d+)?";
                    string pattern = @"/v\d+";
                    requestedUrlWithoutApiVersion = Regex.Replace(requestedUrl, pattern, "");
                }
                else
                    requestedUrlWithoutApiVersion = requestedUrl;

                if (userAuthorizeConfig.ApiPermissions != null && userAuthorizeConfig.ApiPermissions.Count > 0)
                {
                    var ApiPermissions = userAuthorizeConfig.ApiPermissions.OrderBy(x => x.Order).ToList();

                    if (!string.IsNullOrEmpty(requestedUrlWithoutApiVersion))
                    {                       
                        ApiPermissions = userAuthorizeConfig.ApiPermissions.Where(x =>
                            !string.IsNullOrEmpty(x.ControllerUrl) &&
                            requestedUrlWithoutApiVersion.Contains(x.ControllerUrl.ToLower())
                        ).OrderBy(x => x.Order).ToList();
                    }

                    if (ApiPermissions == null || ApiPermissions.Count <= 0)
                    {                       
                        ApiPermissions = userAuthorizeConfig.ApiPermissions.Where(x =>
                            !string.IsNullOrEmpty(x.ControllerUrl) &&
                            requestedUrl.Contains(x.ControllerUrl.ToLower())
                        ).OrderBy(x => x.Order).ToList();
                    }

                    if (appConfig != null)
                    {
                        if (ApiPermissions.Count > 1 && !string.IsNullOrEmpty(AppConst.AppId))
                        {
                            if (ApiPermissions.Any(x => !string.IsNullOrEmpty(x.AppId) && 
                                x.AppId.Equals(AppConst.AppId)))
                                controllerConfig = ApiPermissions.Find(x => !string.IsNullOrEmpty(x.AppId) && x.AppId.Equals(appConfig.Id));
                        }
                        else if (ApiPermissions.Count > 0)
                            controllerConfig = ApiPermissions.FirstOrDefault();
                    }

                    if (controllerConfig == null || (controllerConfig != null && !controllerConfig.IsActive))
                    {
                        if (appConfig != null && appConfig.IsAppAllowed && appConfig.IsAnyApiAllowed)
                            isPermissionValidated = true;
                        else if (userAuthorizeConfig.IsAnyApiAllowed)
                            isPermissionValidated = true;
                        else
                            isPermissionValidated = false;
                    }
                    else if (controllerConfig != null && controllerConfig.IsActive)
                    {
                        if (controllerConfig.ExpiredOn.HasValue && 
                            controllerConfig.ExpiredOn.Value < DateTime.UtcNow)
                            return false;

                        if (controllerConfig.IsPermissionAllowed)
                            isPermissionValidated = true;
                        else
                            isPermissionValidated = false;
                            
                        if (controllerConfig.ConditionalPermissions != null && controllerConfig.ConditionalPermissions.Count > 0)                          
                            isPermissionValidated = await VerifyConditionalApiAccess(context,httpContext, controllerConfig.ConditionalPermissions, isPermissionValidated);

                        if (controllerConfig.EndPointPermissions != null &&  controllerConfig.EndPointPermissions.Count > 0)
                        {
                            controllerConfig.EndPointPermissions = controllerConfig.EndPointPermissions.OrderBy(x => x.Order).ToList();

                            if (!string.IsNullOrEmpty(requestedUrlWithoutApiVersion))
                            {                       
                                endPointConfig = controllerConfig.EndPointPermissions.Find(x =>
                                    !string.IsNullOrEmpty(x.EndPointUrl) &&
                                    requestedUrlWithoutApiVersion.Contains(x.EndPointUrl.ToLower())
                                );
                            }

                            if (endPointConfig == null)
                            {                       
                                endPointConfig = controllerConfig.EndPointPermissions.Find(x =>
                                    !string.IsNullOrEmpty(x.EndPointUrl) &&
                                    requestedUrl.Contains(x.EndPointUrl.ToLower())
                                );
                            }

                            if (endPointConfig == null || (endPointConfig != null && !endPointConfig.IsActive))
                            {
                                if (!controllerConfig.IsPermissionAllowed)
                                    isPermissionValidated = false;
                            }
                            else if (endPointConfig != null & endPointConfig.IsActive)
                            {
                                if (endPointConfig.ExpiredOn.HasValue && 
                                    endPointConfig.ExpiredOn.Value < DateTime.UtcNow)
                                    return false;

                                if (endPointConfig.IsPermissionAllowed)
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;

                                if (endPointConfig.ConditionalPermissions != null && endPointConfig.ConditionalPermissions.Count > 0)
                                    isPermissionValidated = await VerifyConditionalApiAccess(context,httpContext, endPointConfig.ConditionalPermissions, isPermissionValidated);
                            }
                        }
                    }

                    if (endPointConfig != null && endPointConfig.ApiVersion > 0 &&
                        !requestedUrl.StartsWith($"/api/v{endPointConfig.ApiVersion}"))
                            return false;
                    else if (controllerConfig != null && controllerConfig.ApiVersion > 0 &&
                        !requestedUrl.StartsWith($"/api/v{controllerConfig.ApiVersion}"))
                            return false;
                    else if (controllerConfig == null && endPointConfig == null)
                    {
                        if (appConfig != null && appConfig.ApiVersion > 0 &&
                            !requestedUrl.StartsWith($"/api/v{appConfig.ApiVersion}"))
                            return false;
                        else if (userAuthorizeConfig.ApiVersion > 0 &&
                            !requestedUrl.StartsWith($"/api/v{userAuthorizeConfig.ApiVersion}"))
                            return false;
                    }

                    if (endPointConfig != null && endPointConfig.IsReadOnlyAccess.HasValue && 
                        !requestedMethod.Equals("get"))
                    {
                        if (endPointConfig.IsReadOnlyAccess.Value || 
                            (controllerConfig.IsReadOnlyAccess.HasValue && controllerConfig.IsReadOnlyAccess.Value))
                        {
                            if (!string.IsNullOrEmpty(endPoint) && !endPoint.StartsWith("get"))
                                isPermissionValidated = false;
                        }                      
                    }
                    else if (controllerConfig != null && controllerConfig.IsReadOnlyAccess.HasValue && 
                        !requestedMethod.Equals("get"))
                    {                          
                        if (controllerConfig.IsReadOnlyAccess.Value && !string.IsNullOrEmpty(endPoint) && !endPoint.StartsWith("get"))
                            isPermissionValidated = false;
                    }
                    else if (appConfig != null && appConfig.IsAppAllowed && 
                        !requestedMethod.Equals("get"))
                    {
                        if (appConfig.IsReadOnlyAccess)
                        {
                            if (!string.IsNullOrEmpty(endPoint) && !endPoint.StartsWith("get"))
                                isPermissionValidated = false;
                        }
                    }
                    else if (userAuthorizeConfig.IsReadOnlyAccess && !requestedMethod.Equals("get"))
                    {                          
                        if (!string.IsNullOrEmpty(endPoint) && !endPoint.StartsWith("get"))
                            isPermissionValidated = false;
                    }
                }
                else
                {                 
                    if (appConfig != null)
                    {
                        if (appConfig.IsAppAllowed && appConfig.IsAnyApiAllowed)
                        {
                            if (appConfig.IsReadOnlyAccess && !requestedMethod.Equals("get"))
                            {
                                if (!string.IsNullOrEmpty(endPoint) && !endPoint.StartsWith("get"))
                                    isPermissionValidated = false;
                                else
                                    isPermissionValidated = true;
                            }
                            else
                                isPermissionValidated = true;
                        }
                        else
                            isPermissionValidated = false;
                    }
                    else if (userAuthorizeConfig.IsAnyApiAllowed)
                    {
                        if (userAuthorizeConfig.IsReadOnlyAccess && !requestedMethod.Equals("get"))
                        {
                            if (!string.IsNullOrEmpty(endPoint) && !endPoint.StartsWith("get"))
                                isPermissionValidated = false;
                            else
                                isPermissionValidated = true;
                        }
                        else
                            isPermissionValidated = true;
                    }
                    else
                        isPermissionValidated = false;
                }
            }

            return isPermissionValidated;
        }

        private async Task<bool> VerifyConditionalApiAccess(AuthorizationHandlerContext context, HttpContext httpContext, List<ConditionalPermission> conditionalPermissions, bool isPermissionValidated)
        {
            List<bool> allConditionResults = new List<bool>();

            if (conditionalPermissions != null && 
                conditionalPermissions.Count > 0)
            {
                conditionalPermissions = conditionalPermissions.OrderBy(x => x.Order).ToList();
                IDictionary<string, object> requestObject = await GetRequestBodyObject(context, httpContext);

                if (requestObject != null && requestObject.Count > 0)
                {               
                    int totalconditionsExecuted = 0;

                    foreach (var conditionalConfig in conditionalPermissions)
                    {
                        if (conditionalConfig.IsActive && 
                            conditionalConfig.ExpiredOn.HasValue && 
                            conditionalConfig.ExpiredOn.Value < DateTime.UtcNow)
                            continue;
                        
                        if (string.IsNullOrEmpty(conditionalConfig.Key))
                            conditionalConfig.Key = "";

                        if (string.IsNullOrEmpty(conditionalConfig.ValueType))
                            conditionalConfig.ValueType = "";

                        if (string.IsNullOrEmpty(conditionalConfig.Value))
                            conditionalConfig.Value = "";
                        else if (!conditionalConfig.IsCaseSensitive)
                            conditionalConfig.Value = conditionalConfig.Value.ToLower();                          

                        if (string.IsNullOrEmpty(conditionalConfig.LogicalOperator))
                            conditionalConfig.LogicalOperator = "";
                        else
                            conditionalConfig.LogicalOperator = conditionalConfig.LogicalOperator.ToLower();
                        
                        if (conditionalConfig.IsActive &&
                            !string.IsNullOrEmpty(conditionalConfig.Key) &&
                            !string.IsNullOrEmpty(conditionalConfig.LogicalOperator) &&
                            requestObject.ContainsKey(conditionalConfig.Key))
                        {
                            if (requestObject[conditionalConfig.Key] == null)
                                requestObject[conditionalConfig.Key] = "";

                            var matchedCondition = requestObject[conditionalConfig.Key];
                            var matchedConditionValue = matchedCondition.ToString();

                            if (!string.IsNullOrEmpty(matchedConditionValue) && !conditionalConfig.IsCaseSensitive)
                                matchedConditionValue = matchedConditionValue.ToLower();

                            if (conditionalConfig.LogicalOperator.Equals("==") ||
                                conditionalConfig.LogicalOperator.Equals(LogicalOperator.EQ.ToString()))
                            {
                                if (matchedConditionValue.Equals(conditionalConfig.Value))
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals("!=") || 
                                conditionalConfig.LogicalOperator.Equals(LogicalOperator.NE.ToString()))
                            {
                                if (!matchedConditionValue.Equals(conditionalConfig.Value))
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals(LogicalOperator.DO.ToString()))
                            {
                                if (matchedConditionValue.Contains(conditionalConfig.Value))
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals(LogicalOperator.DONT.ToString()))
                            {
                                if (!matchedConditionValue.Contains(conditionalConfig.Value))
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals(LogicalOperator.SW.ToString()))
                            {
                                if (matchedConditionValue.StartsWith(conditionalConfig.Value))
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals(LogicalOperator.DSW.ToString()))
                            {
                                if (!matchedConditionValue.StartsWith(conditionalConfig.Value))
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals(LogicalOperator.EW.ToString()))
                            {
                                if (matchedConditionValue.EndsWith(conditionalConfig.Value))
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals(LogicalOperator.DEW.ToString()))
                            {
                                if (!matchedConditionValue.EndsWith(conditionalConfig.Value))
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals(LogicalOperator.NOE.ToString()))
                            {
                                if (string.IsNullOrEmpty(matchedConditionValue))
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals(LogicalOperator.VAL.ToString()))
                            {
                                if (!string.IsNullOrEmpty(matchedConditionValue))
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals(LogicalOperator.EX.ToString()))
                            {
                                if (conditionalConfig.Value.Contains(matchedConditionValue))
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals(LogicalOperator.DEX.ToString()))
                            {
                                if (!conditionalConfig.Value.Contains(matchedConditionValue))
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals(LogicalOperator.IN.ToString()))
                            {
                                var paramValues = conditionalConfig.Value.Split(',').ToList();

                                if (paramValues.Contains(matchedConditionValue))
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals(LogicalOperator.NIN.ToString()))
                            {
                                var paramValues = conditionalConfig.Value.Split(',').ToList();

                                if (!paramValues.Contains(matchedConditionValue))
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals(LogicalOperator.TYP.ToString()))
                            {
                                var type = matchedCondition.GetType();

                                if (type != null && type.Name.ToLower().Contains(conditionalConfig.ValueType.ToLower()))
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals(LogicalOperator.NTYP.ToString()))
                            {
                                var type = matchedCondition.GetType();

                                if (type != null && !type.Name.ToLower().Contains(conditionalConfig.ValueType.ToLower()))
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals(">") || 
                                conditionalConfig.LogicalOperator.Equals(LogicalOperator.GT.ToString()))
                            {
                                int requestNumber = int.TryParse(matchedConditionValue, out requestNumber) ? requestNumber : 0;
                                int paramLogicNumber = int.TryParse(conditionalConfig.Value.ToString(), out paramLogicNumber) ? paramLogicNumber : 0;

                                if (requestNumber > paramLogicNumber)
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals(">=") || 
                                conditionalConfig.LogicalOperator.Equals(LogicalOperator.GTE.ToString()))
                            {
                                int requestNumber = int.TryParse(matchedConditionValue, out requestNumber) ? requestNumber : 0;
                                int paramLogicNumber = int.TryParse(conditionalConfig.Value.ToString(), out paramLogicNumber) ? paramLogicNumber : 0;

                                if (requestNumber >= paramLogicNumber)
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals("<") || 
                                conditionalConfig.LogicalOperator.Equals(LogicalOperator.LT.ToString()))
                            {
                                int requestNumber = int.TryParse(matchedConditionValue, out requestNumber) ? requestNumber : 0;
                                int paramLogicNumber = int.TryParse(conditionalConfig.Value.ToString(), out paramLogicNumber) ? paramLogicNumber : 0;

                                if (requestNumber < paramLogicNumber)
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }
                            else if (conditionalConfig.LogicalOperator.Equals("<=") || 
                                conditionalConfig.LogicalOperator.Equals(LogicalOperator.LTE.ToString()))
                            {
                                int requestNumber = int.TryParse(matchedConditionValue, out requestNumber) ? requestNumber : 0;
                                int paramLogicNumber = int.TryParse(conditionalConfig.Value.ToString(), out paramLogicNumber) ? paramLogicNumber : 0;

                                if (requestNumber <= paramLogicNumber)
                                    isPermissionValidated = true;
                                else
                                    isPermissionValidated = false;
                            }

                            allConditionResults.Add(isPermissionValidated);

                            if (totalconditionsExecuted > 0 && conditionalConfig.IsAnd)
                                allConditionResults[totalconditionsExecuted] = allConditionResults[totalconditionsExecuted - 1] && isPermissionValidated;
                            else if (totalconditionsExecuted > 0 && !conditionalConfig.IsAnd)
                                allConditionResults[totalconditionsExecuted] = allConditionResults[totalconditionsExecuted - 1] || isPermissionValidated;

                            totalconditionsExecuted++;
                        }
                    }
                }                                  
            }

            if (allConditionResults != null && allConditionResults.Count > 0)
                return allConditionResults.LastOrDefault();

            return isPermissionValidated;
        }
    }
}
