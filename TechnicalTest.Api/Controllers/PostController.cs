using Microsoft.AspNetCore.Mvc;
using TechnicalTest.Api.Models;
using TechnicalTest.Application.Abstractions.Services;
using TechnicalTest.Application.Commands;

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
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreatePostRequest post)
        {
            var command = new CreatePostCommand(
                post.AuthorId,
                post.Title,
                post.Description,
                post.Content);

            var createdPost = await _commandHandler.CreatePostAsync(command);
            return Ok(createdPost);
        }
    }
}

