using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Auth.Models.Configs
{
    public class SignInWith
    {
        public bool GitHub { get; set; } = false;
        public bool LinkedIn { get; set; } = false;
        public bool Microsoft { get; set; } = false;
        public bool Google { get; set; } = false;
        public bool Facebook { get; set; } = false;      
        public bool Twitter { get; set; } = false;
        public bool WhatsApp { get; set; } = false;
    }
}
