using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Mzstruct.Base.Contracts.IContexts
{
    public interface IDbConnContext
    {
        IDbConnection CreateConnection();
        Task<IDbConnection> CreateConnectionAsync();
    }
}
