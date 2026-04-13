using FluentAssertions;
using TechnicalTest.Application.DTOs;
using TechnicalTest.Infrastructure.Models;
using TechnicalTest.Infrastructure.Repositories;
using TechnicalTest.TestHelpers.Builders.Application;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Test.Repositories.WithPostRepository.WhenGetAsync
{
    public class WithExistingPost : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly PostRepository _sut;

        private readonly PostReadModel _postReadModel;
        private readonly AuthorReadModel _authorReadModel;
        private readonly PostDto _postDto;
        private readonly bool _includeAuthor;

        public WithExistingPost()
        {
            _includeAuthor = true;

            _postDto = PostDtoBuilder.Default()
                .Build();

            _postReadModel = new()
            {
                Id = _postDto.Id,
                AuthorId = _postDto.AuthorId,
                Title = _postDto.Title,
                Description = _postDto.Description,
                Content = _postDto.Content,
            };

            _authorReadModel = new()
            {
                Id = _postDto.AuthorId,
                Name = _postDto.Author!.Name,
                Surname = _postDto.Author.Surname
            };

            _dbContext = new TestDbContextFactory().Context;
            _sut = new PostRepository(_dbContext);
        }

        [Fact]
        public async Task ThenMustReturnExpectedPostAndAuthorWhenIncludeAuthorTrue()
        {
            var result = await _sut.GetPostAsync(_postReadModel.Id, _includeAuthor);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(_postDto);
        }

        [Fact]
        public async Task ThenMustReturnExpectedPostWithoutAuthorWhenIncludeAuthorFalse()
        {
            var result = await _sut.GetPostAsync(_postReadModel.Id, _includeAuthor!);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(_postDto, options => options.Excluding(x => x.Author));
        }


        #region Setup/Teardown
        public async Task InitializeAsync()
        {
            await _dbContext.Posts.AddAsync(_postReadModel);
            await _dbContext.Authors.AddAsync(_authorReadModel);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
        #endregion
    }
}
