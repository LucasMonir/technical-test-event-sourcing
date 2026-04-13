using TechnicalTest.Application.Commands;

namespace TechnicalTest.Application.Abstractions.Services
{
    public interface IAuthorResolver
    {
        Task<Guid> ResolveAsync(CreatePostCommand command, CancellationToken cancellationToken = default);
    }
}
