using Mzstruct.Base.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Contracts.IRepos
{
    public interface IMongoRepository
    {
        Task<int> GetAllCount();
        Task<string> SaveMany(IEnumerable<BaseEntity> records);
    }
}
