using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using TechnicalTest.Application.DTOs;
using TechnicalTest.Infrastructure.Persistence;
using TechnicalTest.TestHelpers.Builders.Application;
using TechnicalTest.TestHelpers.Builders.Domain;

namespace TechnicalTest.E2E.Test.E2ETests
{
    public class PostCreateE2ETests : IClassFixture<TechnicalTestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly AppDbContext _dbContext;

        public PostCreateE2ETests(TechnicalTestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();

            var scope = factory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        }


        [Fact]
        public async Task CreateNewPostWithNewAuthorShouldReturnExpectedValues()
        {
            var authorRequest = CreateAuthorRequestBuilder.Default()
                .Build();

            var postRequest = CreatePostRequestBuilder.Default()
                .WithAuthor(authorRequest)
                .Build();

            var expectedAuthor = new AuthorDto(Guid.NewGuid(),
                authorRequest.Name,
                authorRequest.Surname);

            var expectedPost = new PostDto(Guid.NewGuid(),
                Guid.NewGuid(),
                postRequest.Title,
                postRequest.Description,
                postRequest.Content,
                expectedAuthor);

            var response = await _client.PostAsJsonAsync($"/post", postRequest);

            var result = await response.Content.ReadFromJsonAsync<PostDto>();

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedPost, options =>
                options.Excluding(post => post.Id)
                .Excluding(post => post.AuthorId)
                .Excluding(post => post.Author)
            );
        }

        [Fact]
        public async Task CreateNewPostWithExistingAuthorShouldReturnExpectedValues()
        {
            var existingAuthor = AuthorBuilder.Default()
                .Build();

            var postRequest = CreatePostRequestBuilder.Default()
                .WithAuthorId(existingAuthor.Id)
                .Build();

            var expectedPost = new PostDto(Guid.NewGuid(),
                existingAuthor.Id,
                postRequest.Title,
                postRequest.Description,
                postRequest.Content);

            await _dbContext.Authors.AddAsync(existingAuthor);
            await _dbContext.SaveChangesAsync();

            var response = await _client.PostAsJsonAsync($"/post", postRequest);
            var result = await response.Content.ReadFromJsonAsync<PostDto>();

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedPost, options =>
                options.Excluding(post => post.Id)
            );
        }
    }
}
