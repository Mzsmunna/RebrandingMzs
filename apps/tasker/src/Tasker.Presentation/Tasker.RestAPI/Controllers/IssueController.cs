using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasker.Application.Interfaces;
using Tasker.Domain.Entities;
using Tasker.Domain.Helper;
using Tasker.Domain.Models;

namespace Tasker.RestAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    //[Route("api/[controller]/[action]")]
    public class IssueController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IssueController> _logger;
        private readonly IIssueRepository _IssueRepository;
        private readonly IUserRepository _userRepository;

        public IssueController(IConfiguration configuration, 
            ILogger<IssueController> logger, 
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
            List<SearchField>? queries = CommonHelperUtility.JsonListDeserialize<SearchField>(searchQueries);
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
        public IActionResult DeleteIssue(string issueId)
        {
            var users = _IssueRepository.DeleteById(issueId);
            return Ok(users);
        }

        [HttpGet("count/{searchQueries?}")]
        //[ActionName("Count")]
        public IActionResult IssuesCount(string searchQueries)
        {
            List<SearchField>? queries = CommonHelperUtility.JsonListDeserialize<SearchField>(searchQueries);
            var Issues = _IssueRepository.GetAllIssueCount(queries).Result;
            return Ok(Issues);
        }

        [HttpGet("status/{userId}")]
        //[ActionName("Status")]
        public IActionResult IssuesStatus(string userId)
        {
            var Issues = _IssueRepository.GetIssueStatByUserId(userId);
            return Ok(Issues);
        }

        [HttpGet("byAssignerCount/{searchQueries?}")]
        //[ActionName("GetIssuesByAssignerCount")]
        public IActionResult GetIssuesByAssignerCount(string searchQueries)
        {
            List<SearchField>? queries = CommonHelperUtility.JsonListDeserialize<SearchField>(searchQueries);
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

        [HttpGet("byAssigner")]
        //[ActionName("GetIssuesByAssigner")]
        public IActionResult GeIssuesByAssigner(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            List<SearchField>? queries = CommonHelperUtility.JsonListDeserialize<SearchField>(searchQueries);
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

        [HttpGet("byAssignedCount/{searchQueries?}")]
        //[ActionName("GetIssuesByAssignedCount")]
        public IActionResult GetAllIssuesByAssignedCount(string searchQueries)
        {
            List<SearchField>? queries = CommonHelperUtility.JsonListDeserialize<SearchField>(searchQueries);
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

        [HttpGet("byAssigned")]
        //[ActionName("GetIssuesByAssigned")]
        public IActionResult GetIssuesByAssigned(int currentPage, int pageSize, string sortField, string sortDirection, string searchQueries)
        {
            List<SearchField>? queries = CommonHelperUtility.JsonListDeserialize<SearchField>(searchQueries);
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
                    issue.Created = new EventLog();

                if (string.IsNullOrEmpty(issue.Id))
                    issue.Created.At = DateTime.UtcNow;
                else if (issue.Modified != null)
                    issue.Modified.At = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(issue.AssignedId))
                {
                    var user = _userRepository.GetUser(issue.AssignedId).Result;
                    if (user != null)
                    {
                        issue.AssignedName = user.FirstName + " " + user.LastName;
                        issue.AssignedImg = user.Img;
                    }
                }

                if (!string.IsNullOrEmpty(issue.Created.By))
                {
                    var user = _userRepository.GetUser(issue.Created.By).Result;
                    if (user != null)
                    {
                        issue.Created.Name = user.FirstName + " " + user.LastName;
                        issue.Created.Image = user.Img;
                    }
                }

                var result = _IssueRepository.Save(issue).Result;
                return Ok(result ?? issue);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
