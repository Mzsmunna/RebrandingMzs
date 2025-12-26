using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Dtos
{
    public class ParamDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string SortField { get; set; }
        public string SortOrder { get; set; }
        public string SearchText { get; set; }
    }
}
