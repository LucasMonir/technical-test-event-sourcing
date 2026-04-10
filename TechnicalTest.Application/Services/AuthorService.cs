using TechnicalTest.Application.Abstractions.Repositories;
using TechnicalTest.Application.Abstractions.Services;
using TechnicalTest.Application.Commands;
using TechnicalTest.Domain;

namespace TechnicalTest.Application.Services
{
    internal class AuthorResolver(IAuthorRepository authorRepository) : IAuthorResolver
    {
        private readonly IAuthorRepository _authorRepository = authorRepository;

        public async Task<Guid> ResolveAsync(CreatePostCommand command)
        {
            if (command.AuthorId.HasValue)
                return await ResolveExistingAsync(command.AuthorId.Value);

            if (command.Author is not null)
                return await CreateNewAsync(command.Author);

            throw new InvalidOperationException("Cannot create post: no author provided.");
        }

        private async Task<Guid> CreateNewAsync(AuthorModel authorModel)
        {
            var name = authorModel.Name?.Trim();
            var surname = authorModel.Surname?.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
                throw new InvalidOperationException("Author name and surname are required.");

            var author = Author.Create(name, surname);

            return await _authorRepository.CreateAuthorAsync(author);
        }

        private async Task<Guid> ResolveExistingAsync(Guid authorId)
        {
            var author = await _authorRepository.GetPostAuthorAsync(authorId);

            if (author is null)
                throw new InvalidOperationException("AuthorId not found.");

            return author.Id;
        }
    }
}
