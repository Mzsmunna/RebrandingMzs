using Mzstruct.Base.Models;
using Mzstruct.DB.Providers.MongoDB.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.DB.Contracts.IRepos
{
    public interface IAuthUserRepo<TIdentity> where TIdentity : class
    {
        Task<TIdentity?> LoginUser(string identity, string password);
        Task<TIdentity?> LoginUser(string identity);
        Task<TIdentity?> RegisterUser(TIdentity user);
        Task<TIdentity?> GetById(string id);
        Task<TIdentity?> SaveAsync(TIdentity entity);
    }
}
