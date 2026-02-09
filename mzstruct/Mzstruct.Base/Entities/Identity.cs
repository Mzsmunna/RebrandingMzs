using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Entities
{
    public class Identity : BaseEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public List<string>? Roles { get; set; }
        public RefreshToken? RefToken { get; set; }
        public string RefreshToken { get; set; } = string.Empty;

        //[BsonIgnore]
        public byte[]? PasswordHash { get; set; }

        //[BsonIgnore]
        public byte[]? PasswordSalt { get; set; }

        //[BsonIgnore]
        public DateTime? TokenCreated { get; set; }

        //[BsonIgnore]
        public DateTime? TokenExpires { get; set; }
    }
}
