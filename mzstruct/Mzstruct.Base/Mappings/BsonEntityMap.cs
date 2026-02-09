using MongoDB.Bson.Serialization;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Mappings
{
    public static class BsonEntityMap
    {
        public static void RegisterCoreEntities()
        {
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
                    map.UnmapMember(x => x.RefToken);
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
