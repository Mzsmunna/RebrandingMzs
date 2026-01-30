
using Mzstruct.Base.Contracts.ICommands;
using Mzstruct.Base.Contracts.IQueries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Contracts.IEvents
{
    public interface IDispatcher
    {
        //Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
        //Task<TResult> CommandAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    }
}
