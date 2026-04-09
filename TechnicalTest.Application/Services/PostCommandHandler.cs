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

        public async Task<PostDto?> Handle(CreatePostCommand command)
        {
            Author author = await GetAuthor(command);

            var post = Post.Create(
                author.Id,
                command.Title,
                command.Description,
                command.Content
            );

            await _postRepository.CreatePostAsync(post);
            return PostMapper.MapToDto(post);
        }

        private async Task<Author> GetAuthor(CreatePostCommand command)
        {
            if (command.AuthorId.HasValue)
            {
                var author = await _authorRepository.GetPostAuthorAsync(command.AuthorId.Value);
                if (author is not null) return author;
            }

            if (command.Author is not null)
            {
                return await _authorRepository.CreateAuthorAsync(Author.Create(
                    command.Author?.Name ?? string.Empty,
                    command.Author?.Surname ?? string.Empty
                ));
            }

            throw new InvalidOperationException("Cannot create post: no author information provided.");
        }
    }
}
