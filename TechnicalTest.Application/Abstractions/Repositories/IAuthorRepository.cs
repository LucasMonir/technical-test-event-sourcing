using TechnicalTest.Domain;

namespace TechnicalTest.Application.Abstractions.Repositories
{
    public interface IAuthorRepository
    {
        Task<Author?> GetPostAuthorAsync(Guid id);
    }
}
