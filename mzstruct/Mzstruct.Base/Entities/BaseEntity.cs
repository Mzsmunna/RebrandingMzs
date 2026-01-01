using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Entities
{
    public abstract class BaseEntity
    {
        public required string Id { get; set; } = Guid.CreateVersion7().ToString();
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // DateTimeOffset.UtcNow;
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public BaseEvent? Created { get; set; } // = new BaseEvent();
        public BaseEvent? Modified { get; set; }
    }
}
