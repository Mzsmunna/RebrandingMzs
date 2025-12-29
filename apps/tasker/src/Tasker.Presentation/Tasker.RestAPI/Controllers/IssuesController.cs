using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mzstruct.Common.Extensions;
using Tasker.Application.Features.Issues;
using Tasker.Application.Contracts.ICommands;
using Tasker.Application.Contracts.IQueries;

namespace Tasker.RestAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    //[Route("api/[controller]/[action]")]
    public class IssuesController(//ILogger<IssuesController> logger,
            IIssueQuery issueQuery,
            IIssueCommand issueCommand) : ControllerBase
    {
        [HttpGet]
        //[ActionName("Issues")]
        public async Task<IActionResult> GetAllIssues(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            var result = await issueQuery.GetAllIssues(currentPage, pageSize, sortField, sortDirection, searchQueries);
            return result.ToActionResult(this);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIssue(string id)
        {
            var result = await issueQuery.GetIssue(id);
            return result.ToActionResult(this);
        }

        [HttpPost]
        public async Task<IActionResult> CreateIssue(IssueModel issue)
        {
            var result = await issueCommand.CreateIssue(issue);
            return result.ToActionResult(this);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateIssue(Issue issue)
        {
            var result = await issueCommand.UpdateIssue(issue);
            return result.ToActionResult(this);
        }

        [HttpDelete, Authorize]
        public async Task<IActionResult> DeleteIssue(string id)
        {
            var result = await issueCommand.DeleteIssue(id);
            return result.ToActionResult(this);
        }

        [HttpGet("Count/{searchQueries?}")]
        public async Task<IActionResult> GetIssuesCount(string searchQueries)
        {
            var result = await issueQuery.GetIssuesCount(searchQueries);
            return result.ToActionResult(this);
        }

        [HttpGet("Status/{userId}")]
        public async Task<IActionResult> GetIssuesStatus(string userId)
        {
            var result = await issueQuery.GetIssuesStatus(userId);
            return result.ToActionResult(this);
        }

        [HttpGet("CountByAssigner/{searchQueries?}")]
        //[ActionName("CountByAssigner")]
        public async Task<IActionResult> GetIssuesCountByAssigner(string searchQueries)
        {
            var result = await issueQuery.GetIssuesCountByAssigner(searchQueries);
            return result.ToActionResult(this);
        }

        [HttpGet("ByAssigner")]
        //[ActionName("ByAssigner")]
        public async Task<IActionResult> GetIssuesByAssigner(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            var result = await issueQuery.GetIssuesByAssigner(currentPage, pageSize, sortField, sortDirection, searchQueries);
            return result.ToActionResult(this);
        }

        [HttpGet("CountByAssigned/{searchQueries?}")]
        public async Task<IActionResult> GetIssuesCountByAssigned(string searchQueries)
        {
            var result = await issueQuery.GetIssuesCountByAssigned(searchQueries);
            return result.ToActionResult(this);
        }

        [HttpGet("ByAssigned")]
        public async Task<IActionResult> GetIssuesByAssigned(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            var result = await issueQuery.GetIssuesByAssigned(currentPage, pageSize, sortField, sortDirection, searchQueries);
            return result.ToActionResult(this);
        }
    }
}
