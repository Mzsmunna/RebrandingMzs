using Kernel.Drivers.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Drivers.Interfaces
{
    public interface IMongoRepository
    {
        #region Common_Methods
        Task<int> GetAllCount();
        Task<string> SaveMany(IEnumerable<IEntity> records);
        #endregion
    }
}
