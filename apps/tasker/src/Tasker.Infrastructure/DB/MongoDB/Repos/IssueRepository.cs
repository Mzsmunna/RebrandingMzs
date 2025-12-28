using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Errors;
using Mzstruct.Base.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Features.Issues;
using Mzstruct.DB.Providers.MongoDB;
using Mzstruct.DB.Providers.MongoDB.Helpers;
using Tasker.Application.Contracts.IRepos;
using Mzstruct.Base.Contracts.IContexts;
using Tasker.Infrastructure.DB.MongoDB.Mappings;

namespace Tasker.Infrastructure.DB.MongoDB.Repos
{
    public class IssueRepository: MongoDBBase<Issue>, IIssueRepository
    {
        private readonly IMongoCollection<Issue> _collection;
        
        public IssueRepository(IMongoDBContext context, IssueEntityMap entityConfig) : base(context, entityConfig)
        {
            _collection = mongoCollection;
        }

        private FilterDefinition<Issue> BuildFilter(string? _id, List<SearchField>? searchQueries = null)
        {
            //var filter = Builders<T>.Filter.Empty;
            var filter = GenericFilter<Issue>.BuildDynamicFilter(_id, searchQueries);
            return filter;
        }

        public async Task<Result<List<Issue>?>> GetAllIssues()
        {
            var filter = Builders<Issue>.Filter.Empty;
            var sort = SortingDefinition.TableSortingFilter<Issue>(); //Builders<Issue>.Sort.Ascending("Title");
            return await _collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<Result<List<dynamic>?>> GetIssueStatByUserId(string userId)
        {
            //var filter = Builders<User>.Filter.Empty;
            var results = await _collection.AsQueryable()
                            //.OrderByDescending(e => e.Email)
                            .Where(x => !string.IsNullOrEmpty(x.AssignedId) && x.AssignedId.Equals(userId))
                            .GroupBy(x => x.Status)
                            .Select(g => new
                            {
                                //Id = g.First().Id,
                                Status = g.First().Status,
                                Count = g.Count()

                            }).ToListAsync();

            return results.Cast<dynamic>().ToList();
        }

        public async Task<Result<List<Issue>?>> GetAllIssuesByAssigner(string assignerId)
        {
            var filter = Builders<Issue>.Filter.Empty;
            var sort = SortingDefinition.TableSortingFilter<Issue>();
            return _collection.Find(filter).Sort(sort).ToList();
        }

        public async Task<Result<List<Issue>?>> GetAllIssuesByAssigned(string assignedId)
        {
            var filter = Builders<Issue>.Filter.Empty;
            var sort = SortingDefinition.TableSortingFilter<Issue>();
            return _collection.Find(filter).Sort(sort).ToList();
        }

        public async Task<Result<Issue?>> GetIssuesById(string id)
        {
            var filter = Builders<Issue>.Filter.Eq("Id", id);
            return _collection.Find(filter).FirstOrDefault();
        }

        public async Task<Result<Issue?>> GetByTitle(string title)
        {
            var filter = Builders<Issue>.Filter.Empty;

            if (!string.IsNullOrEmpty(title) && title.ToLower() != "undefined")
            {
                title = title.ToLower();
                filter = filter & Builders<Issue>.Filter.Regex("Title", new BsonRegularExpression("/^" + title.Replace("+", @"\+") + "$/i"));
                return _collection.Find(filter).FirstOrDefault();
            }
            else
            {
                return IssueError.EmptyTitle;
            }
        }

        public async Task<Result<Issue?>> Save(BaseEntity entity)
        {
            var issue = entity as Issue;

            if (issue != null)
            {
                if (issue.Status.ToLower().Equals("pending"))
                {
                    issue.IsActive = false;
                    issue.IsDeleted = false;
                    issue.IsCompleted = false;
                }
                else if (issue.Status.ToLower().Equals("in-progress"))
                {
                    issue.IsActive = true;
                    issue.IsDeleted = false;
                    issue.IsCompleted = false;
                }
                else if (issue.Status.ToLower().Equals("done"))
                {
                    issue.IsActive = true;
                    issue.IsCompleted = true;
                    issue.IsDeleted = false;
                }
                else if (issue.Status.ToLower().Equals("discarded"))
                {
                    issue.IsActive = false;
                    issue.IsCompleted = false;
                    issue.IsDeleted = true;
                }

                entity = issue;
                var result = await SaveAsync(entity); //.GetAwaiter().GetResult();
                return issue;
            }

            return ClientError.BadRequest;
        }

        public async Task<Result<bool>> DeleteById(string _id)
        {
            var filter = BuildFilter(_id);
            //var data = _collection.Find(filter).FirstOrDefault();
            DeleteResult result = await _collection.DeleteManyAsync(filter);
            return true;
        }

        #region Common_Methods

        public async Task<Result<Issue?>> GetById(string id)
        {
            var filter = BuildFilter(id);
            return await _collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<Result<List<Issue>?>> GetAllByField(string fieldName, string fieldValue)
        {
            var filter = Builders<Issue>.Filter.Eq(fieldName, fieldValue);
            var result = await _collection.Find(filter).ToListAsync().ConfigureAwait(false);
            //var result = await _collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
            return result;
        }

        public async Task<Result<long>> GetAllIssueCount(List<SearchField>? searchQueries = null)
        {
            var filter = BuildFilter(null, searchQueries);
            return await _collection.Find(filter).CountDocumentsAsync().ConfigureAwait(false);
        }

        public async Task<Result<List<Issue>?>> GetAllIssues(int currentPage, int pageSize, string sortField, string sortDirection, List<SearchField>? searchQueries = null)
        {
            var filter = BuildFilter(null, searchQueries);
            return await _collection.Find(filter).Skip(currentPage * pageSize).Limit(pageSize).ToListAsync().ConfigureAwait(false);
        }

        public async Task<Result<string>> SaveMany(IEnumerable<Issue> records)
        {
            var returnVal = string.Empty;
            await _collection.InsertManyAsync(records);
            return returnVal;
        }

        public async Task<Result<int>> GetAllCount()
        {
            int count = 0;
            var filter = BuildFilter(null);
            count = Convert.ToInt32(await _collection.Find(filter).CountDocumentsAsync());
            return count;
        }

        #endregion
    }
}
