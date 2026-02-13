using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Entities
{
    public class Location //: BaseEntity
    {
        public required string Lat { get; set; }
        public required string Long { get; set; }
        public required string GeoHash { get; set; }
        public required string Address { get; set; }
        public string? Region { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? Province { get; set; }
        public string? County { get; set; }
        public string? Division { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Area { get; set; }
        public string? Zip { get; set; }
        public string? Post { get; set; }
        public string? Block { get; set; }
        public string? Street { get; set; }
        public string? Building { get; set; }
        public string? Tower { get; set; }
        public string? House { get; set; }
        public string? Flat { get; set; }
        public string? Room { get; set; }
        
        // Navigation properties
        public Country? CountryInfo { get; set; }
    }
}
