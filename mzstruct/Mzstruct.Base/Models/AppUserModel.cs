using System.ComponentModel.DataAnnotations;

namespace Mzstruct.Base.Models
{
    public record AppUserModel : BaseModel
    {
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        [Range(0,120)] public int? Age { get; set; }
        [Required] public string Role { get; set; } = string.Empty;
        [Required] public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Img { get; set; } = string.Empty;
    }
}
