using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using Mzstruct.DB.Providers.MongoDB.Contracts.IContexts;
using Mzstruct.DB.Providers.MongoDB.Contracts.IRepos;
using Mzstruct.DB.Providers.MongoDB.Helpers;
using Mzstruct.DB.Providers.MongoDB.Mappers;

namespace Mzstruct.DB.Providers.MongoDB.Repos
{
    public class BaseUserRepository: MongoDBRepo<BaseUser>, IBaseUserRepository
    {
        //private readonly IMongoCollection<User> _collection;
        
        public BaseUserRepository(IMongoDBContext dbContext, BaseUserEntityMap entityConfig) : 
            base(dbContext, entityConfig) { }

        public override FilterDefinition<BaseUser> BuildFilter(string? id, List<SearchField>? searchQueries = null)
        {
            //var filter = Builders<T>.Filter.Empty;
            var filter = GenericFilter<BaseUser>.BuildDynamicFilter(id, searchQueries);
            return filter;
        }

        public async Task<BaseUser?> LoginUser(string email, string password)
        {
            var filter = Builders<BaseUser>.Filter.Empty;
            filter &= Builders<BaseUser>.Filter.Eq(x => x.IsActive, true);
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                filter &= Builders<BaseUser>.Filter.Eq(x => x.Email, email.ToLower());
                filter &= Builders<BaseUser>.Filter.Eq(x => x.Password, password);
                var user = await _collection.Find(filter).FirstOrDefaultAsync();
                return user;
            }
            return null;
        }

        public async Task<BaseUser?> LoginUser(string email)
        {
            var filter = Builders<BaseUser>.Filter.Empty;
            filter &= Builders<BaseUser>.Filter.Eq(x => x.IsActive, true);
            if (!string.IsNullOrEmpty(email))
            {
                filter &= Builders<BaseUser>.Filter.Eq(x => x.Email, email.ToLower());
                var user =  await _collection.Find(filter).FirstOrDefaultAsync();
                return user;
            }
            return null;
        }

        public async Task<BaseUser?> RegisterUser(BaseUser user)
        {
            var filter = Builders<BaseUser>.Filter.Empty;
            //filter &= Builders<User>.Filter.Eq(x => x.IsActive, true);
            if (user is null) return user;
            if (!string.IsNullOrEmpty(user.Email))
            {
                filter &= Builders<BaseUser>.Filter.Eq(x => x.Email, user.Email.ToLower());
                var existingUser = await _collection.Find(filter).FirstOrDefaultAsync();
                if (existingUser != null)
                {
                    existingUser.Password = "?";
                    return null; //return Error.Conflict("User.Exists", "This email already exists.");
                }
                return await Save(user);
            }
            return null;
        }

        public async Task<Result<List<BaseUser>>> GetUsers(string clientId, string adminId)
        {
            var filter = Builders<BaseUser>.Filter.Empty;
            if (!string.IsNullOrEmpty(clientId))
                filter &= Builders<BaseUser>.Filter.Eq("ClientId", ObjectId.Parse(clientId));
            if (!string.IsNullOrEmpty(adminId))
                filter &= Builders<BaseUser>.Filter.Eq("AdminUserId", ObjectId.Parse(adminId));
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

        public override async Task<BaseUser?> Save(BaseUser user)
        {
            if (user == null) return user;
            user.Gender = user.Gender?.ToLower() ?? "";
            user.Email = user.Email.ToLower();
            user.Role = user.Role.ToLower();
            var result = await SaveAsync(user);
            return result;
        }

        public async Task<Result<BaseUser?>> UpdateUser(BaseUser User)
        {
            ObjectId _id = ObjectId.Parse(User.Id);
            var filter = Builders<BaseUser>.Filter.Eq("Id", _id);
            var update = Builders<BaseUser>.Update
                //.Set(x => x.ClientId, User.ClientId)
                //.Set(x => x.AdminUserId, User.AdminUserId)
                .Set(x => x.IsActive, User.IsActive)
                //.Set(x => x.Guides, User.Guides)
                .Set("ModifiedOn", DateTime.Now);
            var result = await _collection.UpdateOneAsync(filter, update);
            return User;
        }
    }
}
