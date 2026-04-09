using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Application.Abstractions.Services;
using TechnicalTest.Application.Commands;
using TechnicalTest.Application.DTOs;
using TechnicalTest.Application.Mappers;
using TechnicalTest.Domain;

namespace TechnicalTest.Application.Services
{
    internal class PostCommandHandler(IAuthorRepository authorRepository, IPostRepository postRepository)
        : IPostCommandHandler
    {
        private readonly IAuthorRepository _authorRepository = authorRepository;
        private readonly IPostRepository _postRepository = postRepository;

        public async Task<PostDto?> CreatePostAsync(CreatePostCommand command)
        {
            var author = await _authorRepository.GetPostAuthorAsync(command.AuthorId);

            var post = new Post()
            {
                Id = Guid.NewGuid(),
                AuthorId = command.AuthorId,
                Title = command.Title,
                Description = command.Description,
                Content = command.Content
            };

            await _postRepository.CreatePostAsync(post);
            return PostMapper.MapToDto(post, author);
        }
    }
}
