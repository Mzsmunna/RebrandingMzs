using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Domain.Entities;
using Tasker.Domain.Models;

namespace Tasker.Application.Interfaces
{
    public interface IMongoRepository
    {
        #region Common_Methods
        Task<int> GetAllCount();
        Task<string> SaveMany(IEnumerable<IEntity> records);
        #endregion
    }
}
