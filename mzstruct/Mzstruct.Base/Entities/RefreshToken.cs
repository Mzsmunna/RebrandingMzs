using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Entities
{
    public class RefreshToken
    {
        public required string JtiId { get; set; }
        public required string Token { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public bool IsRevoked => RevokedAt != null; //&& DateTime.UtcNow >= ExpiresAt;
    }
}
