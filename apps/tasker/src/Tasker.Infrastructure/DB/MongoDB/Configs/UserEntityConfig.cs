using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Mzstruct.Base.Contracts.IConfigs;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Contracts;
using Tasker.Application.Features.Issues;
using Tasker.Application.Features.Users;

namespace Tasker.Infrastructure.DB.MongoDB.Configs
{
    public class UserEntityConfig : MongoEntityConfig<User> //, IMongoEntityConfig
    {
        private readonly string _collectionName = "User";

        public override string Register()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
            {
                BsonClassMap.RegisterClassMap<User>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    
                    //map.MapProperty(x => x.Id).SetElementName("_id");
                    //map.GetMemberMap(x => x.Id).SetSerializer(new StringSerializer(BsonType.ObjectId));

                    //map.MapProperty(x => x.CreatedBy).SetElementName("CreatedBy");
                    //map.GetMemberMap(x => x.CreatedBy).SetSerializer(new StringSerializer(BsonType.ObjectId));

                    //map.MapProperty(x => x.ModifiedBy).SetElementName("ModifiedBy");
                    //map.GetMemberMap(x => x.ModifiedBy).SetSerializer(new StringSerializer(BsonType.ObjectId));
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
