using TechnicalTest.Application.DTOs;
using TechnicalTest.Domain;

namespace TechnicalTest.Application.Mappers
{
    internal class AuthorMapper
    {
        public static AuthorDto? MapToDto(Author? author)
        {
            return author is null ? null
                : new AuthorDto(author.Id, author.Name, author.Surname);
        }
    }
}
