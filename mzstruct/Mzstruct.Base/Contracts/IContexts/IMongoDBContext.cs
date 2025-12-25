using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Contracts.IContexts
{
    public interface IMongoDBContext
    {
        IMongoCollection<TEntity> MapCollectionEntity<TEntity>(string collectionName) where TEntity : class;
    }
}
