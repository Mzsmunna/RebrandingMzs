using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Entities
{
    public class LinkedProfile //: BaseEntity
    {
        public required string UserId { get; set; }
        public required string LinkId { get; set; }
        public required string LinkType { get; set; } // resource / connection names : player' | 'umpire' | 'commentary' | 'supportStaff' | 'scorer' | 'analyst';
    }
}
