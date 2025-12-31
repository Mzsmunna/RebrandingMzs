using Mzstruct.DB.Providers.MongoDB.Contracts.IRepos;
using Tasker.Application.Features.Issues;

namespace Tasker.Application.Contracts.IRepos
{
    public interface IIssueRepository : IMongoDBRepo<Issue>
    {
        Task<List<dynamic>> GetIssueStatByUserId(string userId);
        Task<List<Issue>> GetAllIssuesByAssigner(string assignerId);
        Task<List<Issue>> GetAllIssuesByAssigned(string assignedId);
        Task<Issue?> GetByTitle(string title);
    }
}
