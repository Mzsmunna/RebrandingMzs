using Mzstruct.Base.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Interfaces
{
    public interface IQueryHandler<TQuery, TResult>
    {
        Task<Result<TResult>> Execute(TQuery query);
    }
}
