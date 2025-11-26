using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Domain.Models;

namespace Tasker.Domain.Errors
{
    public static class DomainErrors
    {
        public static readonly Error MissingId = new("ID_Missing", "");
        public static readonly Error NotFound = new("NOT_Found", "");
    }
}
