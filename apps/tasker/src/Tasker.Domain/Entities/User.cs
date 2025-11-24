using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Tasker.Domain.Entities
{
    public class User : IEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public int? Age { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Img { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool? IsActive { get; set; } = false;

        [BsonIgnore]
        public byte[]? PasswordHash { get; set; }

        [BsonIgnore]
        public byte[]? PasswordSalt { get; set; }

        [BsonIgnore]
        public DateTime? TokenCreated { get; set; }

        [BsonIgnore]
        public DateTime? TokenExpires { get; set; }

        public string RefreshToken { get; set; } = string.Empty;
    }
}
