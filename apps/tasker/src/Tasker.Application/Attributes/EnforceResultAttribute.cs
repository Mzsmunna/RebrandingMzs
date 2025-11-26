using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class EnforceResultAttribute : Attribute { }
}
