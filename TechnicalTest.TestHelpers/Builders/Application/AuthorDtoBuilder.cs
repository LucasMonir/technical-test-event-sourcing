using TechnicalTest.Application.DTOs;

namespace TechnicalTest.TestHelpers.Builders.Application
{
    public class AuthorDtoBuilder
    {
        private readonly Guid _id;
        private readonly string _name;
        private readonly string _surname;

        public static AuthorDtoBuilder Default() => new();

        private AuthorDtoBuilder()
        {
            _id = Guid.NewGuid();
            _name = "Name";
            _surname = "Surname";
        }

        public AuthorDto Build() => new(
            _id,
            _name,
            _surname);
    }
}