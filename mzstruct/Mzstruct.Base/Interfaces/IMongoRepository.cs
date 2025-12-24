using Mzstruct.Base.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Interfaces
{
    public interface IMongoRepository
    {
        Task<int> GetAllCount();
        Task<string> SaveMany(IEnumerable<BaseEntity> records);
    }
}
