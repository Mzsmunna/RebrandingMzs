using Microsoft.AspNetCore.Identity.Data;
using Mzstruct.Base.Entities;
using Mzstruct.DB.Providers.MongoDB.Contracts.IMappers;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Configuration.Provider;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
//using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Mzstruct.DB.Providers.MongoDB.Mappers
{
    public class MongoEntityMap : IMongoEntityMap
    {
        private ImmutableDictionary<Type, IEntityClassMap> _entityMaps { get; set; }

        public MongoEntityMap()
        {
            BsonEntityMap.RegisterCoreEntities();
            _entityMaps = new Dictionary<Type, IEntityClassMap>
            {
                { typeof(BaseUser), new BaseUserEntityMap() },

            }.ToImmutableDictionary();
        }

        public IEntityClassMap? GetEntityMap(Type type)
        {
            if (_entityMaps.ContainsKey(type))
                return _entityMaps[type];
            return null;
        }
    }
}
