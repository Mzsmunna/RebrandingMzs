using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Mzstruct.DB.Providers.MongoDB.Mappings;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Contracts;
using Tasker.Application.Features.Issues;

namespace Tasker.Infrastructure.DB.MongoDB.Mappings
{
    public class IssueEntityMap : MongoEntityMap<Issue> //, IMongoEntityConfig
    {
        public IssueEntityMap(string? collectionName = "") : base(collectionName) { }
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
