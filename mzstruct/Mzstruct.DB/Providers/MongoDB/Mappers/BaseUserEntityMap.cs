using MongoDB.Bson.Serialization;
using Mzstruct.Base.Entities;
using Mzstruct.DB.Providers.MongoDB.Helpers;

namespace Mzstruct.DB.Providers.MongoDB.Mappers
{
    public class BaseUserEntityMap : MongoEntityMap<BaseUser> //, IMongoEntityConfig
    {
        public BaseUserEntityMap(string? collectionName = "User") : base(collectionName) { }

        public override string RegisterEntity()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(BaseUser)))
            {
                BsonClassMap.RegisterClassMap<BaseUser>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    
                    //map.MapProperty(x => x.Id).SetElementName("_id");
                    //map.GetMemberMap(x => x.Id).SetSerializer(new StringSerializer(BsonType.ObjectId));

                    //map.MapProperty(x => x.CreatedBy).SetElementName("CreatedBy");
                    //map.GetMemberMap(x => x.CreatedBy).SetSerializer(new StringSerializer(BsonType.ObjectId));

                    //map.MapProperty(x => x.ModifiedBy).SetElementName("ModifiedBy");
                    //map.GetMemberMap(x => x.ModifiedBy).SetSerializer(new StringSerializer(BsonType.ObjectId));

                    //map.GetMemberMap(x => x.LastName).SetSerializer(new StringEncrypter());
                });
            }

            //if (!BsonClassMap.IsClassMapRegistered(typeof(Guide)))
            //{
            //    BsonClassMap.RegisterClassMap<Guide>(child1 =>
            //    {
            //        child1.AutoMap();
            //        child1.SetIgnoreExtraElements(true);
            //    });
            //}

            return _collectionName;
        }
    }
}
