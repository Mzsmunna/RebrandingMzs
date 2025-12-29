using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Entities
{
    public class BaseEntity
    {
        public string Id { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime ActivatedAt { get; set; } = DateTime.UtcNow; // DateTimeOffset.UtcNow;
        public DateTime? DeactivatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
