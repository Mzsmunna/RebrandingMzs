using Kernel.Drivers.Interfaces;
using Kernel.Resources.DAL.MongoDB.Configs;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Resources.DAL.MongoDB
{
    public class MongoDBContext : IMongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(MongoDBConfig mongoDbConfig)
        {
            if(_database == null)
            {
                var client = new MongoClient(mongoDbConfig.ConnectionString);
                _database = client.GetDatabase(mongoDbConfig.DatabaseName);
            }
        }

        public IMongoCollection<TEntity> MapCollectionEntity<TEntity>(string collectionName) where TEntity : class
        {
            if (!string.IsNullOrEmpty(collectionName))
                return _database.GetCollection<TEntity>(collectionName);

            return _database.GetCollection<TEntity>(typeof(TEntity).Name);
        }
    }
}
