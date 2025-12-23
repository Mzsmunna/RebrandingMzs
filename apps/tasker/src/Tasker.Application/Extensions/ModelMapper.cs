using Mzstruct.Base.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Domain.Dtos;
using Tasker.Domain.Entities;
using Tasker.Domain.Models;

namespace Tasker.Application.Extensions
{
    public static class ModelMapper
    {
        public static readonly JsonSerializerSettings jsonSrlizeConfig = new JsonSerializerSettings { Error = (se, ev) => { ev.ErrorContext.Handled = true; } };
        
        public static TModel ToModel<TModel, TEntity>(this TEntity entity) where TEntity : IEntity where TModel : class
        {
            return JsonConvert.DeserializeObject<TModel>(JsonConvert.SerializeObject(entity), jsonSrlizeConfig)!;
        }

        public static TEntity ToEntity<TEntity, TModel>(this TModel dto) where TEntity : IEntity where TModel : class
        {
            return JsonConvert.DeserializeObject<TEntity>(JsonConvert.SerializeObject(dto), jsonSrlizeConfig)!;
        }
    }
}
