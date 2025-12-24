using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Errors;
using Mzstruct.Base.Interfaces;
using Mzstruct.Base.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Errors;
using Tasker.Infrastructure.DB.MongoDB.Configs;
using Mzstruct.DB.Providers.MongoDB;
using Mzstruct.DB.Providers.MongoDB.Helpers;
using Mzstruct.DB.Providers.MongoDB.Models;
using Tasker.Application.Features.Users;
using Tasker.Application.Contracts.IRepos;

namespace Tasker.Infrastructure.DB.MongoDB.Repos
{
    public class UserRepository: MongoDBBase<User>, IUserRepository
    {
        private readonly IMongoCollection<User> _collection;
        public UserRepository(IMongoDBContext dbContext, UserEntityConfig entityConfig) : base(dbContext, entityConfig)
        {
            _collection = mongoCollection;
        }

        private FilterDefinition<User> BuildFilter(string? id, List<SearchField>? searchQueries = null)
        {
            //var filter = Builders<T>.Filter.Empty;
            var filter = GenericFilter<User>.BuildDynamicFilter(id, searchQueries);
            return filter;
        }

        public async Task<Result<User>> LoginUser(string email, string password)
        {
            var filter = Builders<User>.Filter.Empty;
            filter &= Builders<User>.Filter.Eq(x => x.IsActive, true);
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                filter &= Builders<User>.Filter.Eq(x => x.Email, email.ToLower());
                filter &= Builders<User>.Filter.Eq(x => x.Password, password);
                var user = await _collection.Find(filter).FirstOrDefaultAsync();
                return user is not null ? user : Error.NotFound();
            }
            return ClientError.BadRequest;
        }

        public async Task<Result<User>> LoginUser(string email)
        {
            var filter = Builders<User>.Filter.Empty;
            filter &= Builders<User>.Filter.Eq(x => x.IsActive, true);
            if (!string.IsNullOrEmpty(email))
            {
                filter &= Builders<User>.Filter.Eq(x => x.Email, email.ToLower());
                var user =  await _collection.Find(filter).FirstOrDefaultAsync();
                return user is not null ? user : Error.NotFound();
            }
            return ClientError.BadRequest;
        }

        public async Task<Result<User>> RegisterUser(User user)
        {
            var filter = Builders<User>.Filter.Empty;
            //filter &= Builders<User>.Filter.Eq(x => x.IsActive, true);
            if (user == null) return ClientError.BadRequest;
            if (!string.IsNullOrEmpty(user.Email))
            {
                filter &= Builders<User>.Filter.Eq(x => x.Email, user.Email.ToLower());
                var existingUser = await _collection.Find(filter).FirstOrDefaultAsync();
                if (existingUser != null)
                {
                    existingUser.Password = "?";
                    return Error.Conflict("User.Exists", "This email already exists.");
                }
                if (user.Created == null)
                    user.Created = new AppEvent();
                return await Save(user);
            }
            return ClientError.BadRequest;
        }

        public async Task<Result<List<User>>> GetAllByField(string fieldName, string fieldValue)
        {
            var filter = Builders<User>.Filter.Eq(fieldName, fieldValue);
            var response = await _collection.Find(filter).ToListAsync().ConfigureAwait(false);
            //var response = await _collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
            return response is not null && response.Count > 0 ? response : Error.NotFound();
        }

        public async Task<Result<long>> GetAllUserCount(List<SearchField>? searchQueries = null)
        {
            var filter = BuildFilter(null, searchQueries);
            return await _collection.Find(filter).CountDocumentsAsync().ConfigureAwait(false);
        }

        public async Task<Result<List<User>>> GetAllUsers(int currentPage, int pageSize, string sortField, string sortDirection, List<SearchField>? searchQueries = null)
        {
            var filter = BuildFilter(null, searchQueries);
            return await _collection.Find(filter).Skip(currentPage * pageSize).Limit(pageSize).ToListAsync().ConfigureAwait(false);
        }

        public async Task<Result<User>> GetUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return ClientError.BadRequest;
            var filter = Builders<User>.Filter.Empty;
            filter &= Builders<User>.Filter.Eq("Id", ObjectId.Parse(id));
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Result<List<User>>> GetUsers(string clientId, string adminId)
        {
            var filter = Builders<User>.Filter.Empty;
            if (!string.IsNullOrEmpty(clientId))
                filter &= Builders<User>.Filter.Eq("ClientId", ObjectId.Parse(clientId));
            if (!string.IsNullOrEmpty(adminId))
                filter &= Builders<User>.Filter.Eq("AdminUserId", ObjectId.Parse(adminId));
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<Result<List<dynamic>>> GetAllUserToAssign()
        {
            //var filter = Builders<User>.Filter.Empty;
            var results = await _collection.AsQueryable()
                            //.OrderByDescending(e => e.Email)
                            .Where(x => !string.IsNullOrEmpty(x.Email))
                            .GroupBy(e => e.Email)
                            .Select(g => new
                            {
                                Id = g.First().Id,
                                Name = g.First().FirstName + " " + g.First().LastName,
                                Email = g.First().Email,
                                Role = g.First().Role,
                                Img = g.First().Img,
                              
                            }).ToListAsync();
            return results.Cast<dynamic>().ToList();
        }

        public async Task<Result<User>> Save(BaseEntity entity)
        {
            var user = entity as User;
            if (user == null) return ClientError.BadRequest;
            if (string.IsNullOrEmpty(user.Id))
                user.Created.At = DateTime.UtcNow;
            else if  (user.Modified != null)
                user.Modified.At = DateTime.UtcNow;
            user.Gender = user.Gender?.ToLower() ?? "";
            user.Email = user.Email.ToLower();
            user.Role = user.Role.ToLower();
            entity = user;
            MongoOperation operation = await SaveAsync(entity);
            return await GetUser(operation.Id);
        }

        public async Task<Result<bool>> DeleteById(string _id)
        {
            var filter = BuildFilter(_id);
            //var data = _collection.Find(filter).FirstOrDefault();
            DeleteResult result = await _collection.DeleteManyAsync(filter);
            return true;
        }

        public async Task<Result<bool>> UpdateUser(User User)
        {
            ObjectId _id = ObjectId.Parse(User.Id);
            var filter = Builders<User>.Filter.Eq("Id", _id);
            var update = Builders<User>.Update
                //.Set(x => x.ClientId, User.ClientId)
                //.Set(x => x.AdminUserId, User.AdminUserId)
                .Set(x => x.IsActive, User.IsActive)
                //.Set(x => x.Guides, User.Guides)
                .Set("ModifiedOn", DateTime.Now);
            var result = await _collection.UpdateOneAsync(filter, update);
            return result.IsAcknowledged;
        }
    }
}
