using Kernel.Drivers.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Interfaces;
using Tasker.Persistence.DAL.MongoDB.Configs;

namespace Tasker.Persistence.DAL.MongoDB
{
    public class MongoDBContext : IMongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(MongoDBConfig dbConfig)
        {
            if(_database == null)
            {
                var client = new MongoClient(dbConfig.ConnectionString);
                _database = client.GetDatabase(dbConfig.DatabaseName);
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
