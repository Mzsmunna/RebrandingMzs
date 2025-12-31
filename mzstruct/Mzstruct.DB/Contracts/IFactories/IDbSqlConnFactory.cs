using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Mzstruct.DB.Contracts.IFactories
{
    public interface IDbSqlConnFactory
    {
        IDbConnection Connect();
        Task<IDbConnection> ConnectAsync(CancellationToken token = default);
    }
}
