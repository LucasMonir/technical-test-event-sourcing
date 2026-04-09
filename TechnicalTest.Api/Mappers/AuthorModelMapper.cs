using TechnicalTest.Api.Models;
using TechnicalTest.Application.Commands;

namespace TechnicalTest.Api.Mappers
{
    public static class AuthorModelMapper
    {
        public static AuthorModel? ToModel(this CreateAuthorRequest? request)
        {
            if (request is null)
                return null;

            return new AuthorModel
            {
                Name = request.Name,
                Surname = request.Surname
            };
        }
    }
}
