using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Notify.Configs
{
    public class TwilioConfig : SmsConfig
    {
        public string Sid { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;
        public string ServiceSid { get; set; } = string.Empty;
    }
}
