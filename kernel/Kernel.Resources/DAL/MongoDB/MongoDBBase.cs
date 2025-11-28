using Kernel.Drivers.Entities;
using Kernel.Drivers.Interfaces;
using Kernel.Resources.DAL.MongoDB.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Resources.DAL.MongoDB
{
    public class MongoDBBase<T> where T : class
    {
        protected readonly IMongoCollection<T> mongoCollection;

        public MongoDBBase(IMongoDBContext dBContext, IMongoEntityConfig entityConfig)
        {
            string collectionName = entityConfig.Register();

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new Exception("Collection Name Should Not be Null");
            }

            mongoCollection = dBContext.MapCollectionEntity<T>(collectionName);
        }

        public async Task<MongoOperation> SaveAsync(IEntity entity)
        {
            ReplaceOneResult? result = null;
            var operation  = new MongoOperation() { Id = string.Empty, IsCompleted = false };

            if (entity.Id != null && !string.IsNullOrEmpty(entity.Id))
            {
                BsonDocument query = new BsonDocument {
                    { "_id" , ObjectId.Parse(entity.Id) }
                };
                
                if (entity.Modified != null)
                    entity.Modified.At = DateTime.UtcNow; // DateTime.Now
                
                var _entity = entity as T;
                
                if (_entity != null)
                {
                    result = await mongoCollection.ReplaceOneAsync(query, _entity).ConfigureAwait(false);
                    operation.Id = entity.Id;
                    operation.IsCompleted = result.IsAcknowledged;
                }
                
                return operation;
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
                {
                    await mongoCollection.InsertOneAsync(_entity).ConfigureAwait(false);
                    operation.Id = entity.Id;
                    operation.IsCompleted = true;
                }

                return operation;
            }
        }

        public async Task<MongoOperation> SaveManyAsync(List<IEntity> entities)
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

        public async Task<MongoOperation> DeleteAsync(IEntity entity)
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
