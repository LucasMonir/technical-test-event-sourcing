using FluentAssertions;
using TechnicalTest.Application.DTOs;
using TechnicalTest.Application.Mappers;
using TechnicalTest.Domain;
using TechnicalTest.TestHelpers.Builders.Domain;

namespace TechnicalTest.Application.Test.Mappers.WithAuthorMapper
{
    public class WhenMappingToDto
    {
        private readonly Author _author;
        private readonly AuthorDto _expected;

        public WhenMappingToDto()
        {
            _author = AuthorBuilder.Default()
                .Build();

            _expected = new AuthorDto(_author.Id,
                _author.Name,
                _author.Surname);

        }

        [Fact]
        public void ThenMustReturnExpectedDto()
        {
            var result = AuthorMapper.MapToDto(_author);
            result.Should().BeEquivalentTo(_expected);
        }

        [Fact]
        public void ThenMustReturnNullWhenAuthorIsNull()
        {
            var result = AuthorMapper.MapToDto(null);
            result.Should().BeNull();
        }
    }
}
