using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using TechnicalTest.Application.DTOs;
using TechnicalTest.TestHelpers.Builders.Application;

namespace TechnicalTest.E2E.Test.E2ETests
{
    [Collection("E2E")]
    public class PostCreateE2ETests
    {
        private readonly TechnicalTestWebApplicationFactory _factory;

        public PostCreateE2ETests(TechnicalTestWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateValidNewPostWithNewAuthorShouldCreateNewPostAndAuthor()
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
            postId.Should().NotBeEmpty();

            var includeAuthor = true;

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
            ), options => options
                .Excluding(x => x.Author!.Id)
            );
        }

        [Fact]
        public async Task CreateValidPostWithExistingAuthorShouldCreatePost()
        {
            using var client = _factory.CreateClient();

            var authorRequest = CreateAuthorRequestBuilder.Default().Build();
            var firstPostRequest = CreatePostRequestBuilder.Default()
                .WithAuthor(authorRequest)
                .Build();

            var firstResponse = await client.PostAsJsonAsync("/post", firstPostRequest);
            firstResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var firstPostId = await firstResponse.Content.ReadFromJsonAsync<Guid>();

            var firstPost = await client.WaitForProjectionAsync<PostDto>(
                url: $"/post/{firstPostId}?includeAuthor=true",
                predicate: p => p?.Author is not null,
                timeoutSeconds: 10);

            var existingAuthorId = firstPost!.AuthorId;

            var secondPostRequest = CreatePostRequestBuilder.Default()
                .WithAuthorId(existingAuthorId)
                .Build();

            var secondResponse = await client.PostAsJsonAsync("/post", secondPostRequest);
            secondResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var secondPostId = await secondResponse.Content.ReadFromJsonAsync<Guid>();

            var secondPost = await client.WaitForProjectionAsync<PostDto>(
                url: $"/post/{secondPostId}?includeAuthor=true",
                predicate: p => p?.Author is not null,
                timeoutSeconds: 10);

            secondPost.Should().NotBeNull();
            secondPost.AuthorId.Should().Be(existingAuthorId);
            secondPost.Author!.Name.Should().Be(authorRequest.Name);
            secondPost.Author!.Surname.Should().Be(authorRequest.Surname);
        }
    }
}

