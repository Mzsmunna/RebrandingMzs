using Mzstruct.Base.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Contracts.IQueries
{
    public interface IQuery<out TResponse>
    {
    }

    public interface IQueryHandler<in TQuery, TResult> where TQuery : notnull, IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query, CancellationToken token = default);
    }
}
