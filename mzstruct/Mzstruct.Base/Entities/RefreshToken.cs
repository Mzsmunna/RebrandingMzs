using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Entities
{
    public class RefreshToken : BaseEntity
    {
        public required string UserId { get; set; }
        public required string JtiId { get; set; }
        public required string Token { get; set; }
        public required DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public bool IsRevoked => RevokedAt != null; //&& DateTime.UtcNow >= ExpiresAt;

        // Navigation properties
        //public BaseUser User { get; set; } = default!;
    }
}
