using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Net;
 
namespace Tasker.RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ValidateNever]
    public class MediaController : ControllerBase
    {
        [Consumes("multipart/form-data")]
        [HttpPost, Authorize]
        //[ActionName("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (!(!string.IsNullOrEmpty(Request.ContentType)
                   && Request.ContentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0))
            {
                return StatusCode((int)HttpStatusCode.UnsupportedMediaType);
            }

            string newFileName = string.Empty;
            string path = string.Empty;

            //var httpContext = _httpContextAccessor.HttpContext;

            //if (httpContext.Request.Form.Files.Count > 0)
            //{
                //for (int i = 0; i < httpContext.Request.Form.Files.Count; i++)
                //{
                    //IFormFile httpPostedFile = httpContext.Request.Form.Files[i];
                    IFormFile httpPostedFile = file;

                    if (httpPostedFile != null)
                    {
                        var ms = new MemoryStream();
                        await httpPostedFile.CopyToAsync(ms);
                        var bytes = ms.ToArray();

                        var fileName = httpPostedFile.FileName;
                        newFileName = Guid.NewGuid().ToString() + "-" + fileName.Replace("\"", string.Empty);

                        path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"wwwroot/UploadedFiles"));
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        using (var fileStream = new FileStream(Path.Combine(path, newFileName), FileMode.Create))
                        {
                            await httpPostedFile.CopyToAsync(fileStream);
                        }

                        path = @"https://localhost:7074/UploadedFiles/" + newFileName; //Path.Combine(path, newFileName);
                    }
                //}
            //}

            //return Ok(ApplicationConstants.BaseUrl + relPath + provider.FileName);
            //return Ok(ApplicationConstants.BaseUrl + relPath.Replace("wwwroot/", string.Empty) + newFileName);
            return Ok(path);
        }
    }
}
