using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Configs
{
    public class OAuthSignIn
    {
        public GitHubAuth? GitHubAuth { get; set; }
        public LinkedInAuth? LinkedInAuth { get; set; }
        public GoogleAuth? GoogleAuth { get; set; }
        public FacebookAuth? FacebookAuth { get; set; }
        public WhatsAppAuth? WhatsAppAuth { get; set; }
        public TwitterAuth? TwitterAuth { get; set; }
    }
}
