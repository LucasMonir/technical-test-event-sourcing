using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
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
            var result = await response.Content.ReadFromJsonAsync<Guid>();

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            result.Should().NotBeEmpty();

            var savedPost = await _dbContext.Posts
                .FirstOrDefaultAsync(p => p.Id == result);

            savedPost.Should().NotBeNull();
            savedPost!.Title.Should().Be(postRequest.Title);
            savedPost.Description.Should().Be(postRequest.Description);
            savedPost.Content.Should().Be(postRequest.Content);
            savedPost.AuthorId.Should().NotBeEmpty();

            var savedAuthor = await _dbContext.Authors
                .FirstOrDefaultAsync(a => a.Id == savedPost!.AuthorId);

            savedAuthor.Should().NotBeNull();
            savedAuthor!.Name.Should().Be(authorRequest.Name);
            savedAuthor.Surname.Should().Be(authorRequest.Surname);
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
            var result = await response.Content.ReadFromJsonAsync<Guid>();

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            result.Should().NotBeEmpty();

            var savedPost = await _dbContext.Posts
                .FirstOrDefaultAsync(p => p.Id == result);

            savedPost.Should().NotBeNull();
            savedPost!.Title.Should().Be(postRequest.Title);
            savedPost.Description.Should().Be(postRequest.Description);
            savedPost.Content.Should().Be(postRequest.Content);
            savedPost.AuthorId.Should().NotBeEmpty();
        }
    }
}
