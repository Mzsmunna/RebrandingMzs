using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Notify.Models
{
    public class TwilioOption
    {
        public string Sid { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;
        public string ServiceSid { get; set; } = string.Empty;
        public string FromPhone { get; set; } = string.Empty;
    }
}
