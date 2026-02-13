using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;

namespace Mzstruct.Base.Entities
{
    public class BaseUser : Identity
    {
        public string? FirstName { get; set; }
        public string? MidName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Age { get; set; }
        public string? Address { get; set; }
        public string? Department { get; set; }
        public string? Designation { get; set; }
        public string? Position { get; set; }
        public string? Img { get; set; }
        public string? Covr { get; set; }

        // Navigation properties
        public UserDetails? Details { get; set; }
        public Location? Location { get; set; }
        public UserSettings? Settings { get; set; }
        //public UserPermission? Permissions { get; set; }
    }

    public class UserDetails //: BaseEntity
    {
        //public required string UserId { get; set; }

        public string? Languages { get; set; } // "abc, xyz"
        public List<Field>? Emails { get; set; }
        public List<Field>? Phones { get; set; }
        public List<Field>? Accounts { get; set; }
        public List<LinkedProfile>? Profiles { get; set; }
        public List<Field>? Addresses { get; set; } // "home:abc, work:xyz"

        // Navigation properties
        //public BaseUser? User { get; set; }
    }

    public class UserSettings //: BaseEntity
    {
        //public required string UserId { get; set; }

        //public bool IsOnline { get; set; } = false;
        public DateTime? OnlineAt { get; set; }

        //public bool IsLive { get; set; } = false;
        public DateTime? LiveAt { get; set; }

        //public bool IsVerified { get; set; } = false;
        public DateTime? VerifiedAt { get; set; }

        //public bool IsLocked { get; set; } = false;
        public DateTime? LockedAt { get; set; }

        //public bool IsBanned { get; set; } = false;
        public DateTime? BannedAt { get; set; }

        //public bool IsSuspended { get; set; } = false;
        public DateTime? SuspendedAt { get; set; }

        //public bool IsRestricted { get; set; } = false;
        public DateTime? RestrictedAt { get; set; }

        //public bool IsTfaEnabled { get; set; } = false;
        public DateTime? TfaEnabledAt { get; set; }

        // Navigation properties
        //public BaseUser? User { get; set; }
    }
}
