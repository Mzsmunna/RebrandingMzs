using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Cache.Providers
{
    public class RedisConfig
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string PrefixName { get; set; } = string.Empty;
    }
}
