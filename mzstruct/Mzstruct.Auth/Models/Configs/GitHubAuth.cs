using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Models.Configs
{
    public class GitHubAuth
    {
        public string Schema { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string CallbackPath { get; set; } = string.Empty;
        public string AuthorizationEndpoint { get; set; } = string.Empty;
        public string TokenEndpoint { get; set; } = string.Empty;
        public string UserInformationEndpoint { get; set; } = string.Empty;
    }
}
