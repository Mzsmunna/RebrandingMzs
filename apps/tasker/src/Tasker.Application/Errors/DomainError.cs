using Mzstruct.Base.Enums;
using Mzstruct.Base.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Errors
{
    public static class DomainError
    {
        public static readonly Error MissingId = new(ErrorType.Missing, "ID.Missing", "");
    }
}
