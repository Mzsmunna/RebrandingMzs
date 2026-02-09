using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Mappings;
using Mzstruct.DB.Providers.MongoDB.Contracts.IMappers;

namespace Mzstruct.DB.Providers.MongoDB.Mappers
{
    public abstract class EntityClassMap<T> : IEntityClassMap where T : BaseEntity
    {
        protected readonly string _collectionName = typeof(T).Name;

        public EntityClassMap(string? collectionName = "")
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

            return _collectionName;
        }
    }
}
