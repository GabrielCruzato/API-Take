using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApiTake.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly string token;
        
        public ApiController(IConfiguration configuration)
        {
            token = configuration["GitHubToken"];
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var productInformation = new ProductHeaderValue("ApiTake");
            var credentials = new Credentials(token);
            var client = new GitHubClient(productInformation, new Uri("https://github.com/"))
            { Credentials = credentials };

            var request = new SearchRepositoriesRequest()
            {
                User = "Takenet",
                Language = Language.CSharp
            };

            var result = await client.Search.SearchRepo(request);
            var orderedByCreationDate = result.Items.OrderBy(x => x.CreatedAt);
            return Ok(orderedByCreationDate.Take(5));
        }
    }
}
