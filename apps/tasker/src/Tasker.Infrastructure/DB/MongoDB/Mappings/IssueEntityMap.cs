using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Mzstruct.DB.Providers.MongoDB.Mappers;
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

        public override string RegisterEntity()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Issue)))
            {
                BsonClassMap.RegisterClassMap<Issue>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    map.UnmapMember(x => x.Assigner);
                    map.UnmapMember(x => x.Assigned);                  
                });
            }

            return _collectionName;
        }
    }
}
