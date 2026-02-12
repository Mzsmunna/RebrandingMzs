using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.DB.Providers.MongoDB.Mappers
{
    public static class BsonEntityMap
    {
        public static void RegisterCoreEntities()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(BaseEntity)))
            {
                BsonClassMap.RegisterClassMap<BaseEntity>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    map.MapProperty(x => x.Id).SetElementName("_id");
                    map.GetMemberMap(x => x.Id).SetSerializer(new StringSerializer(BsonType.ObjectId));

                    //map.MapProperty(x => x.CreatedBy).SetElementName("CreatedBy");
                    //map.GetMemberMap(x => x.CreatedBy).SetSerializer(new StringSerializer(BsonType.ObjectId));

                    //map.MapProperty(x => x.ModifiedBy).SetElementName("ModifiedBy");
                    //map.GetMemberMap(x => x.ModifiedBy).SetSerializer(new StringSerializer(BsonType.ObjectId));
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(BaseActivity)))
            {
                BsonClassMap.RegisterClassMap<BaseActivity>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    //map.MapMember(x => x.Type).SetRepresentation(BsonType.String);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(BaseEvent)))
            {
                BsonClassMap.RegisterClassMap<BaseEvent>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    //map.MapMember(x => x.Type).SetRepresentation(BsonType.String);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(RefreshToken)))
            {
                BsonClassMap.RegisterClassMap<RefreshToken>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    //map.UnmapMember(x => x.RefreshToken);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Identity)))
            {
                BsonClassMap.RegisterClassMap<Identity>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    //map.UnmapMember(x => x.RefToken);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(ModifiedField)))
            {
                BsonClassMap.RegisterClassMap<ModifiedField>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    map.UnmapMember(x => x.PropertyInfo);
                });
            }
        }
    }
}
