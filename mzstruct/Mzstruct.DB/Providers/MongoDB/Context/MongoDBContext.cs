using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Mzstruct.DB.Providers.MongoDB.Configs;
using Mzstruct.DB.Providers.MongoDB.Contracts.IContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.DB.Providers.MongoDB.Context
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

        public IMongoCollection<TEntity> MapEntity<TEntity>(string collectionName) where TEntity : class
        {
            if (string.IsNullOrEmpty(collectionName))
                collectionName = typeof(TEntity).Name;

            if (!BsonClassMap.IsClassMapRegistered(typeof(TEntity)))
            {
                BsonClassMap.RegisterClassMap<TEntity>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    //map.MapProperty(x => x.Id).SetElementName("_id");
                    //map.GetMemberMap(x => x.Id).SetSerializer(new StringSerializer(BsonType.ObjectId));
                    //map.GetMemberMap(x => x.LastName).SetSerializer(new StringEncrypter());
                });
            }

            return _database.GetCollection<TEntity>(collectionName);
        }
    }
}
