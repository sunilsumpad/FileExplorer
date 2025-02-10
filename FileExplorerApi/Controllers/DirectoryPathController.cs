using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
                var result = GetDirectoryTree(path);
                return Ok(result); // Return JSON response
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // Recursive function to get directory structure
        private object GetDirectoryTree(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            return new
            {
                Directory = directoryInfo.Name,
                SubDirectories = directoryInfo.GetDirectories()
                    .Where(d =>!d.Name.StartsWith('.'))
                    .Select(d => GetDirectoryTree(d.FullName))
                    .ToList(),
                Files = directoryInfo.GetFiles()
                    .Where(d =>!d.Name.StartsWith('.'))
                    .Select(f => f.Name).ToList()
            };
        }
    }
}
