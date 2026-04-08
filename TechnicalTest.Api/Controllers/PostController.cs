using Microsoft.AspNetCore.Mvc;
using TechnicalTest.Application.Abstractions.Services;

namespace TechnicalTest.Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PostController(IPostQueryService queryService) : ControllerBase
    {
        private readonly IPostQueryService _queryService = queryService;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var post = await _queryService.GetPostAsync(id);
            return Ok(post);
        }
    }
}
