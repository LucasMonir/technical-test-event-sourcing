using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using TechnicalTest.Application.DTOs;
using TechnicalTest.TestHelpers.Builders.Application;

namespace TechnicalTest.E2E.Test.E2ETests
{
    [Collection("E2E")]
    public class PostGetE2ETests : IAsyncLifetime
    {
        private readonly TechnicalTestWebApplicationFactory _factory;

        public PostGetE2ETests(TechnicalTestWebApplicationFactory factory)
        {
            _factory = factory;
        }

        public async Task InitializeAsync()
        {
            await _factory.ResetDatabaseAsync();
        }
        public Task DisposeAsync() => Task.CompletedTask;


        [Fact]
        public async Task GetPostIncludingAuthorReturnsPostWithAuthor()
        {
            using var client = _factory.CreateClient();

            var authorRequest = CreateAuthorRequestBuilder.Default()
                .Build();

            var postRequest = CreatePostRequestBuilder.Default()
                .WithAuthor(authorRequest)
                .Build();

            var includeAuthor = true;

            var response = await client.PostAsJsonAsync("/post", postRequest);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var postId = await response.Content.ReadFromJsonAsync<Guid>();

            var post = await client.WaitForProjectionAsync<PostDto>(
                url: $"/post/{postId}?includeAuthor={includeAuthor}",
                predicate: p => p is not null,
                timeoutSeconds: 10);

            post.Should().NotBeNull();
            post.Should().BeEquivalentTo(new PostDto(
                postId,
                post.AuthorId,
                postRequest.Title,
                postRequest.Description,
                postRequest.Content,
                new AuthorDto(
                    post.AuthorId,
                    authorRequest.Name,
                    authorRequest.Surname
                )
            ), options => options.Excluding(x => x.Author!.Id));
        }

        [Fact]
        public async Task GetPostIncludingAuthorReturnsPostWithoutAuthor()
        {
            using var client = _factory.CreateClient();

            var authorRequest = CreateAuthorRequestBuilder.Default()
                .Build();

            var postRequest = CreatePostRequestBuilder.Default()
                .WithAuthor(authorRequest)
                .Build();

            var response = await client.PostAsJsonAsync("/post", postRequest);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var postId = await response.Content.ReadFromJsonAsync<Guid>();

            var post = await client.WaitForProjectionAsync<PostDto>(
                url: $"/post/{postId}",
                predicate: p => p is not null,
                timeoutSeconds: 10);

            post.Should().BeEquivalentTo(new PostDto(
                postId,
                post.AuthorId,
                postRequest.Title,
                postRequest.Description,
                postRequest.Content
            ), options => options.Excluding(x => x.AuthorId));
        }
    }
}
