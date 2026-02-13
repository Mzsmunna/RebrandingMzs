using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using Mzstruct.Base.Patterns.Result;
using Mzstruct.DB.Contracts.IRepos;
using Mzstruct.DB.Providers.MongoDB.Contracts.IContexts;
using Mzstruct.DB.Providers.MongoDB.Contracts.IMappers;
using Mzstruct.DB.Providers.MongoDB.Contracts.IRepos;
using Mzstruct.DB.Providers.MongoDB.Helpers;

namespace Mzstruct.DB.Providers.MongoDB.Repos
{
    public class BaseUserRepository<TUser>: MongoDBRepo<TUser>, 
        IBaseUserRepository<TUser>, 
        IAuthUserRepo<TUser> where TUser : BaseUser
    {
        //private readonly IMongoCollection<User> _collection;
        
        public BaseUserRepository(IMongoDBContext dbContext, IMongoEntityMap dbEntities) : 
            base(dbContext, dbEntities) { }

        public override FilterDefinition<TUser> BuildFilter(string? id, List<SearchField>? searchQueries = null)
        {
            //var filter = Builders<T>.Filter.Empty;
            var filter = GenericFilter<TUser>.BuildDynamicFilter(id, searchQueries);
            return filter;
        }

        public async Task<TUser?> LoginUser(string identity, string password) // identity -> username or email or phone
        {
            var filter = Builders<TUser>.Filter.Empty;
            filter &= Builders<TUser>.Filter.Eq(x => x.IsActive, true);

            if (!string.IsNullOrEmpty(identity) && !string.IsNullOrEmpty(password))
            {
                identity = identity.ToLower();
                //filter &= Builders<TUser>.Filter.Eq(x => x.Email, identity);
                filter &= Builders<TUser>.Filter.Or(
                    Builders<TUser>.Filter.Eq(x => x.Username, identity),
                    Builders<TUser>.Filter.Eq(x => x.Email, identity),
                    Builders<TUser>.Filter.Eq(x => x.Phone, identity)
                );
                filter &= Builders<TUser>.Filter.Eq(x => x.Password, password);
                var user = await collection.Find(filter).FirstOrDefaultAsync();
                return user;
            }
            return null;
        }

        public async Task<TUser?> LoginUser(string identity) // identity -> username or email or phone
        {
            var filter = Builders<TUser>.Filter.Empty;
            filter &= Builders<TUser>.Filter.Eq(x => x.IsActive, true);
            if (!string.IsNullOrEmpty(identity))
            {
                identity = identity.ToLower();
                //filter &= Builders<TUser>.Filter.Eq(x => x.Email, identity);
                filter &= Builders<TUser>.Filter.Or(
                    Builders<TUser>.Filter.Eq(x => x.Username, identity),
                    Builders<TUser>.Filter.Eq(x => x.Email, identity),
                    Builders<TUser>.Filter.Eq(x => x.Phone, identity)
                );
                var user =  await collection.Find(filter).FirstOrDefaultAsync();
                return user;
            }
            return null;
        }

        public async Task<TUser?> RegisterUser(TUser user)
        {
            if (user is null) return user;
            
            if (string.IsNullOrEmpty(user.Username)) user.Username = user.Email.ToLower();
            user.Username = user.Username.ToLower();
            user.Email = user.Email.ToLower();
            user.Role = user.Role.ToLower();
            user.Phone = user.Phone?.ToLower() ?? "";
            user.IsActive = true;
            user.IsDeleted = false;

            var filter = Builders<TUser>.Filter.Empty;

            if (!string.IsNullOrEmpty(user.Username))
            {
                filter = filter | Builders<TUser>.Filter.Eq(x => x.Username, user.Username);
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                //filter &= Builders<TUser>.Filter.Eq(x => x.Email, user.Email.ToLower());
                filter = filter | Builders<TUser>.Filter.Eq(x => x.Email, user.Email);
            }

            if (!string.IsNullOrEmpty(user.Phone))
            {
                filter = filter | Builders<TUser>.Filter.Eq(x => x.Phone, user.Phone);
            }

            //filter &= Builders<User>.Filter.Eq(x => x.IsActive, true);

            //var f = Builders<TUser>.Filter;

            //var orFilters = new List<FilterDefinition<TUser>>
            //{
            //    f.Eq(x => x.Username, user.Username.ToLower()),
            //    f.Eq(x => x.Email, user.Email.ToLower())
            //};

            //// Add phone only if it has value
            //if (!string.IsNullOrWhiteSpace(user.Phone))
            //{
            //    orFilters.Add(f.Eq(x => x.Phone, user.Phone));
            //}

            //filter &= f.Or(orFilters);

            var existingUser = await collection.Find(filter).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                //existingUser.Password = "?";
                return null; //return Error.Conflict("User.Exists", "This email already exists.");
            }
            return await Save(user);
        }

        public async Task<Result<List<TUser>>> GetUsers(string clientId, string adminId)
        {
            var filter = Builders<TUser>.Filter.Empty;
            if (!string.IsNullOrEmpty(clientId))
                filter &= Builders<TUser>.Filter.Eq("ClientId", ObjectId.Parse(clientId));
            if (!string.IsNullOrEmpty(adminId))
                filter &= Builders<TUser>.Filter.Eq("AdminUserId", ObjectId.Parse(adminId));
            return await collection.Find(filter).ToListAsync();
        }

        public async Task<Result<List<dynamic>>> GetAllUserToAssign()
        {
            //var filter = Builders<User>.Filter.Empty;
            var results = await collection.AsQueryable()
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

        public override async Task<TUser?> Save(TUser user)
        {
            if (user == null) return user;
            user.Gender = user.Gender?.ToLower() ?? "";
            user.Email = user.Email.ToLower();
            user.Role = user.Role.ToLower();
            var result = await SaveAsync(user);
            return result;
        }

        public async Task<Result<TUser?>> UpdateUser(TUser User)
        {
            ObjectId _id = ObjectId.Parse(User.Id);
            var filter = Builders<TUser>.Filter.Eq("Id", _id);
            var update = Builders<TUser>.Update
                //.Set(x => x.ClientId, User.ClientId)
                //.Set(x => x.AdminUserId, User.AdminUserId)
                .Set(x => x.IsActive, User.IsActive)
                //.Set(x => x.Guides, User.Guides)
                .Set("ModifiedOn", DateTime.Now);
            var result = await collection.UpdateOneAsync(filter, update);
            return User;
        }
    }
}
