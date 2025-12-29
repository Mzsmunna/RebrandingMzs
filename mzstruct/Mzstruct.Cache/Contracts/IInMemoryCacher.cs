using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Cache.Contracts
{
    public interface IInMemoryCacher
    {
        T GetData<T>(string key);
        T SetData<T>(string key, T value, DateTimeOffset expirationTime);
        bool RemoveData<T>(string key);
    }
}
