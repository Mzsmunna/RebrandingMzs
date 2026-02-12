using Mzstruct.Base.Models;
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
        public string? Phone { get; set; }
        //public List<Field>? Roles { get; set; } // role name & permissions

        // Navigation properties
        public RefreshToken? RefreshJWT { get; set; }
    }
}
