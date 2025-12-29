using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Entities
{
    public class Identity : MongoEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public required string Password { get; set; }

        //[BsonIgnore]
        public byte[]? PasswordHash { get; set; }

        //[BsonIgnore]
        public byte[]? PasswordSalt { get; set; }

        //[BsonIgnore]
        public DateTime? TokenCreated { get; set; }

        //[BsonIgnore]
        public DateTime? TokenExpires { get; set; }

        public string RefreshToken { get; set; } = string.Empty;
    }
}
