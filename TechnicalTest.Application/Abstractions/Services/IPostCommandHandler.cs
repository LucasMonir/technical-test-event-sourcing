using TechnicalTest.Application.Commands;
using TechnicalTest.Application.DTOs;

namespace TechnicalTest.Application.Abstractions.Services
{
    public interface IPostCommandHandler
    {
        Task<PostDto?> Handle(CreatePostCommand command);
    }
}
