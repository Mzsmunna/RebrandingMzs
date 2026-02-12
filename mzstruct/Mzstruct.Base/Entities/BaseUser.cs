using Mzstruct.Base.Entities;

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
    }
}
