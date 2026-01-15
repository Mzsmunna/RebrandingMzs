using Microsoft.AspNetCore.Identity;
using Mzstruct.Base.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.DB.EFCore.Entities
{
    public class UserEntity : IdentityUser
    {
        //public required string Id { get; set; } = Guid.CreateVersion7().ToString();
        public required string Name { get; set; }
        public required string Role { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool IsActive { get; set; } = true;
        public List<string>? Roles { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // DateTimeOffset.UtcNow;
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public BaseEvent? Created { get; set; } // = new BaseEvent();
        public BaseEvent? Modified { get; set; }
    }
}
