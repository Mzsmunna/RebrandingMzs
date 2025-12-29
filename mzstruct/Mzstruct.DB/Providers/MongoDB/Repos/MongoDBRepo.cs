using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using Mzstruct.Base.Contracts.IRepos;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using Mzstruct.DB.Providers.MongoDB.Contracts.IContexts;
using Mzstruct.DB.Providers.MongoDB.Contracts.IMappers;
using Mzstruct.DB.Providers.MongoDB.Contracts.IRepos;
using Mzstruct.DB.Providers.MongoDB.Helpers;
using Mzstruct.DB.Providers.MongoDB.Models;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.DB.Providers.MongoDB.Repos
{
    public class MongoDBRepo<T> : IMongoDBRepo<T> where T : BaseEntity //class
    {
        protected readonly IMongoCollection<T> _collection;

        public MongoDBRepo(IMongoDBContext dBContext, IMongoEntityMap entityMap)
        {
            string collectionName = entityMap.RegisterEntity();
            if (string.IsNullOrEmpty(collectionName))
                throw new Exception("Collection Name Should Not be Null");
            _collection = dBContext.MapCollectionEntity<T>(collectionName);
        }

        public virtual FilterDefinition<T> BuildFilter(string? id, List<SearchField>? searchQueries = null)
        {
            //var filter = Builders<T>.Filter.Empty;
            var filter = GenericFilter<T>.BuildDynamicFilter(id, searchQueries);
            return filter;
        }

        public virtual async Task<T?> Get(MongoDBParam query)
        {
            var result = await _collection.Find(query.Parameters).FirstOrDefaultAsync();
            return result == null ? null : result as T;
        }

        public virtual async Task<T?> GetById(string id)
        {
            var filter = BuildFilter(id);
            return await _collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public virtual async Task<List<T>> GetByFieldValue(string fieldName, string fieldValue)
        {
            var filter = Builders<T>.Filter.Eq(fieldName, fieldValue);
            var result = await _collection.Find(filter).ToListAsync().ConfigureAwait(false);
            return result ?? [];
        }

        public async Task<List<T>> GetAll(string sortField = "", string sortDirection = "")
        {
            var filter = Builders<T>.Filter.Empty;
            var sort = SortingDefinition.TableSortingFilter<T>(sortField, sortDirection); //Builders<T>.Sort.Ascending("Title");
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public virtual async Task<List<T>> GetAll(MongoDBParam query)
        {
            var results = await _collection.Find(query.Parameters).ToListAsync();
            return results ?? [];
        }

        public virtual async Task<List<T>> GetAll(int currentPage, int pageSize, string sortField, string sortDirection, List<SearchField>? searchQueries = null)
        {
            var filter = BuildFilter(null, searchQueries);
            var result = await _collection.Find(filter).Skip(currentPage * pageSize).Limit(pageSize).ToListAsync().ConfigureAwait(false);
            return result ?? [];
        }

        public virtual async Task<T?> SortAndGet(MongoDBParam query, string sortingFieldName)
        {
            var sort = Builders<T>.Sort.Descending(sortingFieldName);
            var result = await _collection.Find(query.Parameters).Sort(sort).FirstOrDefaultAsync();
            return result == null ? null : result;
        }

        public virtual async Task<long> GetTotalCount()
        {
            var filter = BuildFilter(null);
            var count = await _collection.Find(filter).CountDocumentsAsync();
            return count;
        }

        public virtual async Task<long> GetCount(List<SearchField>? searchQueries = null)
        {
            var filter = BuildFilter(null, searchQueries);
            var count = await _collection.Find(filter).CountDocumentsAsync().ConfigureAwait(false);
            return count;
        }

        public async Task<T?> SaveAsync(T entity)
        {
            ReplaceOneResult? result = null;
            var operation  = new MongoOperation() { 
                Id = entity.Id ?? ObjectId.GenerateNewId().ToString(),
                IsCompleted = false 
            };

            if (entity == null)
                return entity;

            if (entity.Created is null)
            {
                entity.Created = new AppEvent(typeof(T).Name, operation.Id);
                entity.Created.Id = ObjectId.GenerateNewId().ToString();
                entity.Created.At = DateTime.UtcNow; // DateTime.Now
            }
            
            if (entity.Modified is null)
            {
                entity.Modified = new AppEvent(typeof(T).Name, operation.Id);
                entity.Modified.Id = ObjectId.GenerateNewId().ToString();
            }
            entity.Modified.At = DateTime.UtcNow; // DateTime.Now

            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = operation.Id; //ObjectId.GenerateNewId().ToString();
                await _collection.InsertOneAsync(entity).ConfigureAwait(false);
                operation.Id = entity.Id;
                operation.IsCompleted = true;
                return entity;
            }
            else {
                var query = new BsonDocument {
                    { "_id" , ObjectId.Parse(entity.Id) }
                };
                result = await _collection.ReplaceOneAsync(query, entity).ConfigureAwait(false);
                operation.Id = entity.Id;
                operation.IsCompleted = result.IsAcknowledged;
                return entity;
            }
        }

        public async Task<long> SaveManyAsync(List<T> entities)
        {
            // initialise write model to hold list of our upsert tasks
            var dataModels = new List<WriteModel<T>>();

            foreach(var entity in entities)
            {
                if (entity == null)
                    continue;

                if (string.IsNullOrEmpty(entity.Id))
                {
                    entity.Id = ObjectId.GenerateNewId().ToString();
                    if (entity.Created != null)
                        entity.Created.At = DateTime.UtcNow; // DateTime.Now
                    if (entity.Modified != null)
                        entity.Modified.At = DateTime.UtcNow; // DateTime.Now                  
                    dataModels.Add(new InsertOneModel<T>(entity));
                }
                else
                {
                    BsonDocument query = new BsonDocument {
                        { "_id" , ObjectId.Parse(entity.Id) }
                    };
                    if (entity.Modified != null)
                        entity.Modified.At = DateTime.UtcNow; // DateTime.Now
                    // use ReplaceOneModel with property IsUpsert set to true to upsert whole documents
                    dataModels.Add(new ReplaceOneModel<T>(query, entity) { IsUpsert = true });
                }
            }

            var result = await _collection.BulkWriteAsync(dataModels);
            return result.InsertedCount;
        }

        public virtual async Task<T?> Save(T entity)
        {
            var result = await SaveAsync(entity);
            return entity;
        }

        public virtual async Task<long> SaveMany(IEnumerable<T> records)
        {
            var result = await SaveManyAsync(records.ToList());
            return result;
        }

        public async Task<T?> DeleteAsync(T entity, bool isSoftDelete = true)
        {           
            if (entity is null || string.IsNullOrEmpty(entity.Id))
                return null;

            BsonDocument query = new BsonDocument {
                { "_id" , ObjectId.Parse(entity.Id) }
            };

            if (isSoftDelete)
            {
                entity.IsDeleted = true;
                var updatedEntity = await SaveAsync(entity);
                return updatedEntity ?? null;
            }
            
            var result = await _collection.DeleteOneAsync(query).ConfigureAwait(false);
            if (result != null && result.DeletedCount > 0)
                return entity;
            else return null;
        }

        public virtual async Task<T?> DeleteById(string id, bool isSoftDelete = true)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            var entity = await GetById(id);
            if (entity != null)
                return await DeleteAsync(entity, isSoftDelete);
            return null;
        }

        public async Task<bool> DeleteManyAsync(string id, bool isSoftDelete = true)
        {
            var filter = BuildFilter(id);
            DeleteResult result = await _collection.DeleteManyAsync(filter);
            return true;
        }
    }
}
