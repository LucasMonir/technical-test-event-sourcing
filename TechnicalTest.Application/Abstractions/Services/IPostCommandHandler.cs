using TechnicalTest.Application.Commands;

namespace TechnicalTest.Application.Abstractions.Services
{
    public interface IPostCommandHandler
    {
        Task<Guid> Handle(CreatePostCommand command);
    }
}
