using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Notify.Contracts
{
    public interface ISmsService
    {
        Task<string> SendSMS(string from, string to, string content);
    }
}
