using System;
using System.Collections.Generic;
using System.Text;

namespace TaskerDomain.Dtos
{
    public class RefreshToken
    {
        public required string Token { get; set; }
        public required DateTime Created { get; set; } = DateTime.UtcNow;
        public required DateTime Expires { get; set; }
    }
}
