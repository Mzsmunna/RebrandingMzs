using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.DB.Providers.MongoDB.Repos
{
    public class MongoDBCollection<T> where T : class
    {
        //public string IdentityKey;
        private bool IsSuccess;
        private readonly IMongoCollection<T> _collection;

        public MongoDBCollection(IMongoCollection<T> baseCollection)
        {
            _collection = baseCollection;
        }

        public async Task<bool> Save(T entity)
        {
            if (entity.GetType().GetProperty("CreatedOn") != null)
                entity.GetType().GetProperty("CreatedOn")?.SetValue(entity, DateTime.Now);
            await _collection.InsertOneAsync(entity);
            IsSuccess = true;

            return IsSuccess;
        }
    }
}
