using Kernel.Drivers.Errors;
using Kernel.Drivers.Models;
using Kernel.Processes.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasker.Application.Errors;
using Tasker.Application.Features.Issues;
using Tasker.Application.Features.Users;
using Tasker.Domain.Entities;
using Tasker.Domain.Models;

namespace Tasker.RestAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    //[Route("api/[controller]/[action]")]
    public class IssuesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IssuesController> _logger;
        private readonly IIssueRepository _IssueRepository;
        private readonly IUserRepository _userRepository;

        public IssuesController(IConfiguration configuration, 
            ILogger<IssuesController> logger, 
            IIssueRepository IssueRepository, 
            IUserRepository userRepository)
        {
            _configuration = configuration;
            _logger = logger;
            _IssueRepository = IssueRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        //[ActionName("Issues")]
        public IActionResult AllIssues(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            List<SearchField>? queries = ProcessHelper.JsonListDeserialize<SearchField>(searchQueries);
            var Issues = _IssueRepository.GetAllIssues(currentPage, pageSize, sortField, sortDirection, queries).Result;
            return Ok(Issues);
        }

        [HttpGet("{id}")]
        //[ActionName("Issues")]
        public IActionResult GetIssue(string id)
        {
            var Issues = _IssueRepository.GetById(id).Result;
            return Ok(Issues);
        }

        [HttpPost]
        //[ActionName("Issues")]
        public IActionResult CreateIssue(Issue issue)
        {
            return SaveIssue(issue);
        }

        [HttpPut]
        //[ActionName("Issues")]
        public IActionResult UpdateIssue(Issue issue)
        {
            return SaveIssue(issue);
        }

        [HttpDelete, Authorize]
        //[ActionName("Issues")]
        public IActionResult DeleteIssue(string id)
        {
            var users = _IssueRepository.DeleteById(id);
            return Ok(users);
        }

        [HttpGet("Count/{searchQueries?}")]
        //[ActionName("Count")]
        public IActionResult GetIssuesCount(string searchQueries)
        {
            List<SearchField>? queries = ProcessHelper.JsonListDeserialize<SearchField>(searchQueries);
            var Issues = _IssueRepository.GetAllIssueCount(queries).Result;
            return Ok(Issues);
        }

        [HttpGet("Status/{userId}")]
        //[ActionName("Status")]
        public IActionResult GetIssuesStatus(string userId)
        {
            var Issues = _IssueRepository.GetIssueStatByUserId(userId);
            return Ok(Issues);
        }

        [HttpGet("CountByAssigner/{searchQueries?}")]
        //[ActionName("CountByAssigner")]
        public IActionResult GetIssuesCountByAssigner(string searchQueries)
        {
            List<SearchField>? queries = ProcessHelper.JsonListDeserialize<SearchField>(searchQueries);
            if (queries != null && queries.Count > 0 && queries.Any(x => !string.IsNullOrEmpty(x.Key) && x.Key.ToLower().Equals("assignerid")))
            {
                var issueCount = _IssueRepository.GetAllIssueCount(queries);
                return Ok(issueCount);
            }
            else
            {
                return Ok(0);
            }
        }

        [HttpGet("ByAssigner")]
        //[ActionName("ByAssigner")]
        public IActionResult GetIssuesByAssigner(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            List<SearchField>? queries = ProcessHelper.JsonListDeserialize<SearchField>(searchQueries);
            if (queries != null && queries.Count > 0 && queries.Any(x => !string.IsNullOrEmpty(x.Key) && x.Key.ToLower().Equals("assignerid")))
            {
                var Issues = _IssueRepository.GetAllIssues(currentPage, pageSize, sortField, sortDirection, queries);
                return Ok(Issues);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("CountByAssigned/{searchQueries?}")]
        //[ActionName("CountByAssigned")]
        public IActionResult GetIssuesCountByAssigned(string searchQueries)
        {
            List<SearchField>? queries = ProcessHelper.JsonListDeserialize<SearchField>(searchQueries);
            if (queries != null && queries.Count > 0 && queries.Any(x => !string.IsNullOrEmpty(x.Key) && x.Key.ToLower().Equals("assignerid")))
            {
                var issueCount = _IssueRepository.GetAllIssueCount(queries);
                return Ok(issueCount);
            }
            else
            {
                return Ok(0);
            }
        }

        [HttpGet("ByAssigned")]
        //[ActionName("ByAssigned")]
        public IActionResult GetIssuesByAssigned(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            List<SearchField>? queries = ProcessHelper.JsonListDeserialize<SearchField>(searchQueries);
            if (queries != null && queries.Count > 0 && queries.Any(x => !string.IsNullOrEmpty(x.Key) && x.Key.ToLower().Equals("assignerid")))
            {
                var Issues = _IssueRepository.GetAllIssues(currentPage, pageSize, sortField, sortDirection, queries);
                return Ok(Issues);
            }
            else
            {
                return NotFound();
            }
        }

        //---
        private IActionResult SaveIssue(Issue issue)
        {
            if (issue != null)
            {
                if (issue.Created == null)
                    issue.Created = new AppEvent();

                if (string.IsNullOrEmpty(issue.Id))
                    issue.Created.At = DateTime.UtcNow;
                else if (issue.Modified != null)
                    issue.Modified.At = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(issue.AssignedId))
                {
                    var response = _userRepository.GetUser(issue.AssignedId).Result;
                    issue = response.Map(
                        Ok: user =>
                        {
                            issue.AssignedName = user.FirstName + " " + user.LastName;
                            issue.AssignedImg = user.Img ?? "";
                            return issue;
                        },
                        Err: _ => issue
                    );
                }

                if (!string.IsNullOrEmpty(issue.Created.By))
                {
                    var response = _userRepository.GetUser(issue.Created.By).Result;
                    issue = response.Map(
                        Ok: user =>
                        {
                            issue.Created.Name = user.FirstName + " " + user.LastName;
                            issue.Created.Image = user.Img;
                            return issue;
                        },
                        Err: _ => issue
                    );
                }

                var result = _IssueRepository.Save(issue).Result;
                return Ok(result);
            }
            else
            {
                return Ok(ClientError.BadRequest);
            }
        }
    }
}
