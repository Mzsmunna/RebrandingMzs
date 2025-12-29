using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Mzstruct.DB.Providers.MongoDB.Mappers
{
    public class BsonEntityMap
    {
        public BsonEntityMap()
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

            if (!BsonClassMap.IsClassMapRegistered(typeof(Identity)))
            {
                BsonClassMap.RegisterClassMap<Identity>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    map.UnmapMember(x => x.PasswordHash);
                    map.UnmapMember(x => x.PasswordSalt);
                    map.UnmapMember(x => x.TokenCreated);
                    map.UnmapMember(x => x.TokenExpires);
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
