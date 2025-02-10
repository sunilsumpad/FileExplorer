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
        public string GetDirectories()
        {
            return "Test";
        }
    }
}
