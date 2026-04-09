using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using TechnicalTest.Application.DTOs;
using TechnicalTest.Infrastructure.Persistence;
using TechnicalTest.TestHelpers.Builders;

namespace TechnicalTest.E2E.Test.E2ETests
{
    public class PostGetE2ETests : IClassFixture<TechnicalTestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly AppDbContext _dbContext;

        public PostGetE2ETests(TechnicalTestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();

            var scope = factory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        }


        [Fact]
        public async Task GetPostIncludingAuthorReturnsPostWithAuthor()
        {
            var author = AuthorBuilder.Default()
                .Build();

            var post = PostBuilder.Default()
                .WithAutorId(author.Id)
                .Build();

            AuthorDto authorDto = GetAuthorDto(author);
            PostDto postDto = GetPostDto(post, authorDto);

            _dbContext.Authors.Add(author);
            _dbContext.Posts.Add(post);

            await _dbContext.SaveChangesAsync();

            var response = await _client.GetAsync($"/post/{post.Id}?includeAuthor=true");

            var result = await response.Content.ReadFromJsonAsync<PostDto>();

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(postDto);
        }

        [Fact]
        public async Task GetPostIncludingAuthorReturnsPostWithoutAuthor()
        {
            var author = AuthorBuilder.Default()
                .Build();

            var post = PostBuilder.Default()
                .WithAutorId(author.Id)
                .Build();

            AuthorDto authorDto = GetAuthorDto(author);
            PostDto postDto = GetPostDto(post, authorDto);

            _dbContext.Authors.Add(author);
            _dbContext.Posts.Add(post);

            await _dbContext.SaveChangesAsync();

            var response = await _client.GetAsync($"/post/{post.Id}");

            var result = await response.Content.ReadFromJsonAsync<PostDto>();

            result.Should().NotBeNull();
            result.Author.Should().BeNull();
            result.Should().BeEquivalentTo(result, options =>
                options.Excluding(post => post.Author)
            );
        }

        private static PostDto GetPostDto(Domain.Post post, AuthorDto authorDto)
            => new(post.Id, post.AuthorId, post.Title, post.Description, post.Content, authorDto);

        private static AuthorDto GetAuthorDto(Domain.Author author)
        => new(author.Id, author.Name, author.Surname);
    }
}
