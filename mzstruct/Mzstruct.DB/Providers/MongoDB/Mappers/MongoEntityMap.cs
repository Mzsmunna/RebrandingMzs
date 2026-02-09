using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Mappings;
using Mzstruct.DB.Providers.MongoDB.Contracts.IMappers;

namespace Mzstruct.DB.Providers.MongoDB.Mappers
{
    public abstract class MongoEntityMap<T> : IMongoEntityMap where T : BaseEntity
    {
        protected readonly string _collectionName = typeof(T).Name;

        public MongoEntityMap(string? collectionName = "")
        {
            _collectionName = collectionName ?? typeof(T).Name;
        }

        public virtual string Register()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(BaseEntity)))
            {
                BsonClassMap.RegisterClassMap<BaseEntity>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    map.MapProperty(x => x.Id).SetElementName("_id");
                    map.GetMemberMap(x => x.Id).SetSerializer(new StringSerializer(BsonType.ObjectId));
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap.RegisterClassMap<T>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    //map.MapProperty(x => x.Id).SetElementName("_id");
                    //map.GetMemberMap(x => x.Id).SetSerializer(new StringSerializer(BsonType.ObjectId));
                    //map.GetMemberMap(x => x.LastName).SetSerializer(new StringEncrypter());
                });
            }

            BsonEntityMap.RegisterCoreEntities();
            return _collectionName;
        }
    }
}
