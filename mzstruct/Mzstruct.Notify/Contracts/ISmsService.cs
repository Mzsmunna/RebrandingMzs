using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Notify.Contracts
{
    public interface ISmsService
    {
        Task<string> Send(string from, string to, string content);
        Task<string> Receive(string from, string to, string content); // most likely webhook
    }
}
