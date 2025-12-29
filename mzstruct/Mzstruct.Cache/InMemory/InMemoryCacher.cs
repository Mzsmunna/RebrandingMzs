using Mzstruct.Cache.Contracts;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using System.Runtime.Caching;

namespace Mzstruct.Cache.InMemory
{
    public class InMemoryCacher : IInMemoryCacher
    {
        private ObjectCache _inMemoryCache = MemoryCache.Default;

        public T GetData<T>(string key)
        {
            try
            {
                T item = (T) _inMemoryCache.Get(key);
                return item;
            }
            catch
            {
                throw;
            }
        }

        public T SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            try
            {
                if (!string.IsNullOrEmpty(key) && value != null)
                {
                    _inMemoryCache.Set(key, value, expirationTime);
                }
                
                return value;
            }
            catch
            {
                throw;
            }
        }

        public bool RemoveData<T>(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    _inMemoryCache.Remove(key);
                    return true;
                }
                
                return false;
            }
            catch
            {
                throw;
            }
        }       
    }
}
