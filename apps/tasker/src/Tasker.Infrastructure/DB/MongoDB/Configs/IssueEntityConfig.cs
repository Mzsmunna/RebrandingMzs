using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Features.Issues;
using Tasker.Application.Contracts;

namespace Tasker.Infrastructure.DB.MongoDB.Configs
{
    public class IssueEntityConfig : MongoEntityConfig<Issue>
    {
        public IssueEntityConfig(string? collectionName = "") : base(collectionName) { }
    }

    //public class IssueEntityConfig : IMongoEntityConfig
    //{
    //    private readonly string _collectionName = "Issue";

    //    public string Register()
    //    {
    //        if (!BsonClassMap.IsClassMapRegistered(typeof(Issue)))
    //        {
    //            BsonClassMap.RegisterClassMap<Issue>(map =>
    //            {
    //                map.AutoMap();
    //                map.SetIgnoreExtraElements(true);
    //                map.MapProperty(x => x.Id).SetElementName("_id");
    //                map.GetMemberMap(x => x.Id).SetSerializer(new StringSerializer(BsonType.ObjectId));
    //            });
    //        }

    //        return _collectionName;
    //    }
    //}
}
