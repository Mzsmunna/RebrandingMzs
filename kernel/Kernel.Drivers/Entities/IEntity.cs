using Kernel.Drivers.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Drivers.Entities
{
    public class IEntity
    {
        public required string Id { get; set; }
        public required AppEvent Created { get; set; }
        public AppEvent? Modified { get; set; }
    }
}
