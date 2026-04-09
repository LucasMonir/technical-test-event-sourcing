using FluentAssertions;
using TechnicalTest.Application.DTOs;
using TechnicalTest.Application.Mappers;
using TechnicalTest.Domain;
using TechnicalTest.TestHelpers.Builders.Domain;

namespace TechnicalTest.Application.Test.Mappers.WhitPostMapper
{
    public class WhenMappingToDto
    {
        private readonly Author _author;
        private readonly Post _post;
        private readonly PostDto _expected;

        public WhenMappingToDto()
        {
            _author = AuthorBuilder.Default()
                .Build();

            _post = PostBuilder.Default()
                .WithAutorId(_author.Id)
                .Build();

            _expected = new PostDto(_post.Id,
                _author.Id,
                _post.Title,
                _post.Description,
                _post.Content,
                new AuthorDto(
                    _author.Id,
                    _author.Name,
                    _author.Surname)
                );
        }

        [Fact]
        public void ThenMustReturnExpectedDtoWithoutAuthorWhenNotIncluded()
        {
            var result = PostMapper.MapToDto(_post);

            result.Should().BeEquivalentTo(new PostDto(_post.Id,
                _author.Id,
                _post.Title,
                _post.Description,
                _post.Content,
                null));
        }

        [Fact]
        public void ThenMustReturnExpectedDtoWithAuthorWhenIncluded()
        {
            var result = PostMapper.MapToDto(_post, _author);

            result.Should().Be(_expected);
        }
    }
}