using MongoDB.Bson.Serialization;
using Mzstruct.Base.Entities;
using Mzstruct.DB.Providers.MongoDB.Contracts.IMappers;

namespace Mzstruct.DB.Providers.MongoDB.Mappers
{
    public abstract class MongoEntityMap<T> : BsonEntityMap, IMongoEntityMap where T : MongoEntity
    {
        protected readonly string _collectionName = typeof(T).Name;

        public MongoEntityMap(string? collectionName = "") : base()
        {
            _collectionName = collectionName ?? typeof(T).Name;
        }

        public virtual string RegisterEntity()
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
