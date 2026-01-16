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
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // DateTimeOffset.UtcNow;
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        //public required string Role { get; set; }
        //public required string Password { get; set; }
        //public List<string>? Roles { get; set; }

        //public BaseEvent? Created { get; set; } // = new BaseEvent();
        //public BaseEvent? Modified { get; set; }

        //public string? FirstName { get; set; }
        //public string? LastName { get; set; }     
        //public string? Gender { get; set; }
        //public DateTime? BirthDate { get; set; }
        //public int? Age { get; set; }
        //public string? Phone { get; set; }
        //public string? Address { get; set; }
        //public string? Department { get; set; }
        //public string? Designation { get; set; }
        //public string? Position { get; set; }
        //public string? Img { get; set; }
        //public string RefreshToken { get; set; } = string.Empty;
        //public byte[]? PasswordSalt { get; set; }
        //public DateTime? TokenCreated { get; set; }
        //public DateTime? TokenExpires { get; set; }
        //public DateTime ActivatedAt { get; set; } = DateTime.UtcNow; // DateTimeOffset.UtcNow;
        //public DateTime? DeactivatedAt { get; set; }
    }
}
