using Microsoft.AspNetCore.Mvc;
using TechnicalTest.Api.Mappers;
using TechnicalTest.Api.Models;
using TechnicalTest.Application.Abstractions.Services;

namespace TechnicalTest.Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PostController(IPostQueryService queryService, IPostCommandHandler commandHandler)
        : ControllerBase
    {
        private readonly IPostQueryService _queryService = queryService;
        private readonly IPostCommandHandler _commandHandler = commandHandler;

        [HttpGet("{id}")]
        public async Task<IActionResult> Post(Guid id, bool includeAuthor = false)
        {
            var post = await _queryService.GetPostAsync(id, includeAuthor);
            return post is not null ? Ok(post) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreatePostRequest post)
        {
            var command = post.ToCommand();

            var createdPost = await _commandHandler.Handle(command);
            return CreatedAtAction(nameof(Post), createdPost);
        }
    }
}

