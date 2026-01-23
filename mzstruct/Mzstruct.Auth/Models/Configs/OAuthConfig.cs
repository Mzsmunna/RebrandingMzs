using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Models.Configs
{
    public abstract class OAuthConfig
    {
        public bool IsEnabled { get; set; } = false;
        public string Schema { get; set; } = "External";
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string CallbackPath { get; set; } = string.Empty;
        public string RedirectPath { get; set; } = string.Empty;
        public string AuthorizationEndpoint { get; set; } = string.Empty;
        public string TokenEndpoint { get; set; } = string.Empty;
        public string UserInformationEndpoint { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string Fields { get; set; } = string.Empty;
    }

    public class GitHubAuth : OAuthConfig { }
    public class LinkedInAuth : OAuthConfig { }
    public class GoogleAuth : OAuthConfig { }
    public class FacebookAuth : OAuthConfig { }
    public class WhatsAppAuth : OAuthConfig { }
    public class TwitterAuth : OAuthConfig { }
}
