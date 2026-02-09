using Mzstruct.Base.Dtos;
using Mzstruct.Base.Entities;
using Mzstruct.Base.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Application.Features.Issues;
using Mzstruct.DB.Providers.MongoDB.Helpers;
using Tasker.Application.Contracts.IRepos;
using Tasker.Infrastructure.DB.MongoDB.Mappings;
using Mzstruct.DB.Providers.MongoDB.Repos;
using Mzstruct.DB.Providers.MongoDB.Contracts.IContexts;

namespace Tasker.Infrastructure.DB.MongoDB.Repos
{
    public class IssueRepository: MongoDBRepo<Issue>, IIssueRepository
    {
        //private readonly IMongoCollection<Issue> _collection;
        
        public IssueRepository(IMongoDBContext context, IssueEntityMap entityConfig) : base(context, entityConfig) { }

        //private FilterDefinition<Issue> BuildFilter(string? _id, List<SearchField>? searchQueries = null)
        //{
        //    //var filter = Builders<T>.Filter.Empty;
        //    var filter = GenericFilter<Issue>.BuildDynamicFilter(_id, searchQueries);
        //    return filter;
        //}

        public async Task<List<dynamic>> GetIssueStatByUserId(string userId)
        {
            //var filter = Builders<User>.Filter.Empty;
            var results = await collection.AsQueryable()
                            //.OrderByDescending(e => e.Email)
                            .Where(x => !string.IsNullOrEmpty(x.UserId) && x.UserId.Equals(userId))
                            .GroupBy(x => x.Status)
                            .Select(g => new
                            {
                                //Id = g.First().Id,
                                Status = g.First().Status,
                                Count = g.Count()

                            }).ToListAsync();

            return results != null ? results.Cast<dynamic>().ToList() : [];
        }

        public async Task<List<Issue>> GetAllIssuesByAssigner(string assignerId)
        {
            var filter = Builders<Issue>.Filter.Empty;
            var sort = SortingDefinition.TableSortingFilter<Issue>();
            var result = collection.Find(filter).Sort(sort).ToList();
            return result ?? [];
        }

        public async Task<List<Issue>> GetAllIssuesByAssigned(string assignedId)
        {
            var filter = Builders<Issue>.Filter.Empty;
            var sort = SortingDefinition.TableSortingFilter<Issue>();
            var result = collection.Find(filter).Sort(sort).ToList();
            return result ?? [];
        }

        public async Task<Issue?> GetByTitle(string title)
        {
            var filter = Builders<Issue>.Filter.Empty;
            if (!string.IsNullOrEmpty(title) && title.ToLower() != "undefined")
            {
                title = title.ToLower();
                filter = filter & Builders<Issue>.Filter.Regex("Title", new BsonRegularExpression("/^" + title.Replace("+", @"\+") + "$/i"));
                return collection.Find(filter).FirstOrDefault();
            }
            return null;
        }

        public override async Task<Issue?> Save(Issue issue)
        {
            if (issue is null)
                return issue;        
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
            var result = await SaveAsync(issue); //.GetAwaiter().GetResult();
            return issue;  
        }
    }
}
