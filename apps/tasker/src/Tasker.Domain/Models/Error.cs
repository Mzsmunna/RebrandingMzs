using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Domain.Models
{
    public record Error(string type, string message, string code = "", string description = "")
    {
        public static Error? None => new(string.Empty, string.Empty, string.Empty, string.Empty);
    }
}
