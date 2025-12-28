using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Mzstruct.Base.Contracts.IMappings;
using Mzstruct.Base.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.DB.Providers.MongoDB.Mappings
{
    public class MongoEntityMap<T> : BsonEntityMap, IMongoEntityMap where T : BaseEntity
    {
        private readonly string _collectionName = typeof(T).Name;

        public MongoEntityMap(string? collectionName = "") : base()
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
