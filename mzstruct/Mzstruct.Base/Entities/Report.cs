using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Entities
{
    public class Report : BaseTask
    {
        /// Type => violation | abuse | spam | harrasment | vulnerability | bug | feedback | other
        /// Status => pending | in_queue | open | in_progress | resolved | closed | rejected
        /// Summary => allegation
        public List<FieldMap>? Claims { get; set; }  // attachment, screenshot, video, audio, document, etc.
        public List<ReferenceMap>? Accused { get; set; }
        public List<ReferenceMap>? Victims { get; set; }
    }
}
