using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Mzstruct.Base.Contracts.IFactories
{
    public interface IDbSqlFactory
    {
        IDbConnection Connect();
        Task<IDbConnection> ConnectAsync();
    }
}
