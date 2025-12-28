using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Mzstruct.DB.Providers.MongoDB.Models;
using Mzstruct.Base.Contracts.IContexts;
using Mzstruct.Base.Contracts.IMappers;

namespace Mzstruct.DB.Providers.MongoDB
{
    public class MongoDBBase<T> where T : class
    {
        protected readonly IMongoCollection<T> mongoCollection;

        public MongoDBBase(IMongoDBContext dBContext, IMongoEntityMap entityMap)
        {
            string collectionName = entityMap.RegisterEntity();

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new Exception("Collection Name Should Not be Null");
            }

            mongoCollection = dBContext.MapCollectionEntity<T>(collectionName);
        }

        public async Task<MongoOperation> SaveAsync(BaseEntity entity)
        {
            ReplaceOneResult? result = null;
            var operation  = new MongoOperation() { Id = string.Empty, IsCompleted = false };

            if (entity.Created is null)
                entity.Created = new AppEvent();
            entity.Created.At = DateTime.UtcNow; // DateTime.Now

            if (entity.Modified is null)
                entity.Modified = new AppEvent();
            entity.Modified.At = DateTime.UtcNow; // DateTime.Now

            var _entity = entity as T;
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = ObjectId.GenerateNewId().ToString();
                if (_entity != null)
                {
                    await mongoCollection.InsertOneAsync(_entity).ConfigureAwait(false);
                    operation.Id = entity.Id;
                    operation.IsCompleted = true;
                }
                return operation;
            }
            else {
                BsonDocument query = new BsonDocument {
                    { "_id" , ObjectId.Parse(entity.Id) }
                };
                if (_entity != null)
                {
                    result = await mongoCollection.ReplaceOneAsync(query, _entity).ConfigureAwait(false);
                    operation.Id = entity.Id;
                    operation.IsCompleted = result.IsAcknowledged;
                }
                return operation;
            }
        }

        public async Task<MongoOperation> SaveManyAsync(List<BaseEntity> entities)
        {
            // initialise write model to hold list of our upsert tasks
            var dataModels = new List<WriteModel<T>>();

            foreach(var entity in entities)
            {
                if (entity.Id != null && !string.IsNullOrEmpty(entity.Id))
                {
                    BsonDocument query = new BsonDocument {
                        { "_id" , ObjectId.Parse(entity.Id) }
                    };

                    if (entity.Modified != null)
                        entity.Modified.At = DateTime.UtcNow; // DateTime.Now

                    var _entity = entity as T;

                    // use ReplaceOneModel with property IsUpsert set to true to upsert whole documents
                    if (_entity != null)
                        dataModels.Add(new ReplaceOneModel<T>(query, _entity) { IsUpsert = true });
                }
                else
                {
                    entity.Id = ObjectId.GenerateNewId().ToString();

                    if (entity.Created != null)
                        entity.Created.At = DateTime.UtcNow; // DateTime.Now

                    if (entity.Modified != null)
                        entity.Modified.At = DateTime.UtcNow; // DateTime.Now

                    var _entity = entity as T;
                    
                    if (_entity != null)
                        dataModels.Add(new InsertOneModel<T>(_entity));
                }
            }

            var result = await mongoCollection.BulkWriteAsync(dataModels);
            return new MongoOperation() { Id = string.Empty, IsCompleted = result.IsAcknowledged };
        }

        public async Task<MongoOperation> DeleteAsync(BaseEntity entity)
        {
            var _entity = entity as T;

            BsonDocument query = new BsonDocument {
                { "_id" , ObjectId.Parse(entity.Id) }
            };

            var result = await mongoCollection.DeleteOneAsync(query).ConfigureAwait(false);
            return new MongoOperation() { Id = entity.Id, IsCompleted = result.IsAcknowledged };
        }
    }
}
