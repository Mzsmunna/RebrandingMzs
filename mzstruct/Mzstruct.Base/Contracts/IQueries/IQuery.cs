using Mzstruct.Base.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Contracts.IQueries
{
    public interface IQuery<out TResponse> where TResponse : class
    {
    }

    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult> where TResult : class
    {
        Task<TResult> HandleAsync(TQuery query, CancellationToken token = default);
    }
}
