using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Notify.Models
{
    public class TwilioReplySms
    {
        public string From { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
