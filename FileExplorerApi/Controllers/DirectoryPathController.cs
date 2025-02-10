using Microsoft.AspNetCore.Mvc;
using FileExplorerApi.DirectoryPathHelpers;

namespace FileExplorerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectoryPathController : ControllerBase
    {
        // GET api/DirectoryPath/directories
        [HttpGet("directories")]
        public IActionResult GetDirectories([FromQuery] string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return BadRequest("Path cannot be empty.");
            }

            try
            {
                if (!Directory.Exists(path))
                {
                    return NotFound("Directory not found.");
                }
                var result = DirectoryPathHelper.GetDirectoryTree(path);
                return Ok(result); // Return JSON response
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
