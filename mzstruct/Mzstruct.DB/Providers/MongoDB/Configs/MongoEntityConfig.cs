using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Mzstruct.Base.Contracts.IConfigs;
using Mzstruct.Base.Entities;
using Mzstruct.DB.Providers.MongoDB.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Infrastructure.DB.MongoDB.Configs
{
    public class MongoEntityConfig<T> : BaseMongoConfig, IMongoEntityConfig where T : BaseEntity
    {
        private readonly string _collectionName = typeof(T).Name;

        public MongoEntityConfig(string? collectionName = "") : base()
        {
            _collectionName = collectionName ?? typeof(T).Name;
        }

        public virtual string Register()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap.RegisterClassMap<T>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    //map.MapProperty(x => x.Id).SetElementName("_id");
                    //map.GetMemberMap(x => x.Id).SetSerializer(new StringSerializer(BsonType.ObjectId));
                });
            }
            return _collectionName;
        }
    }
}
