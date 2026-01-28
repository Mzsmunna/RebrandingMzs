using Mzstruct.Base.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzstruct.Notify.Configs
{
    public class EmailConfig
    {
        public string UserName
        {
            get; // => Decrypt(value);
            set => field = value;
        } = string.Empty;
        public string Password
        {
            get => field; //Decrypt(value);
            set => field = value;
        } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool EnableSSL { get; set; } = false;
        public bool EnableTLS { get; set; } = true;
        public List<Field>? Options { get; set; } = [];
    }
}
