using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Mzstruct.DB.Resources.MongoDB.Models;

namespace Mzstruct.DB.Resources.MongoDB
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

        public async Task<T?> Get(MongoDBParam query)
        {
            var result = await _collection.Find(query.Parameters).FirstOrDefaultAsync();
            return result == null ? null : result as T;
        }

        public async Task<T?> SortAndGet(MongoDBParam query, string sortingFieldName)
        {
            var sort = Builders<T>.Sort.Descending(sortingFieldName);
            var result = await _collection.Find(query.Parameters).Sort(sort).FirstOrDefaultAsync();
            return result == null ? null : result as T;
        }

        public async Task<List<T>?> GetAll(MongoDBParam query)
        {
            var results = await _collection.Find(query.Parameters).ToListAsync();
            return results == null ? null : results;
        }
    }
}
