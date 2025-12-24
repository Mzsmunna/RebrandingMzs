using Mzstruct.Base.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Text;
using Mzstruct.Base.Entities;

namespace Tasker.Infrastructure.DB.MongoDB.Configs
{
    public class MongoEntityConfig<T> : IMongoEntityConfig where T : BaseEntity //class
    {
        private readonly string _collectionName = typeof(T).Name;

        public MongoEntityConfig(string? collectionName = "")
        {
            _collectionName = collectionName ?? typeof(T).Name;
        }

        public string Register()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap.RegisterClassMap<T>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    map.MapProperty(x => x.Id).SetElementName("_id");
                    map.GetMemberMap(x => x.Id).SetSerializer(new StringSerializer(BsonType.ObjectId));
                });
            }

            return _collectionName;
        }
    }
}
