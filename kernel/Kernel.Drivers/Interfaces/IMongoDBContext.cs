using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Drivers.Interfaces
{
    public interface IMongoDBContext
    {
        IMongoCollection<TEntity> MapCollectionEntity<TEntity>(string collectionName) where TEntity : class;
    }
}
