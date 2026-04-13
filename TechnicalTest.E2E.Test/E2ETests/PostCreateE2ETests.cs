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
        public async Task CreateNewPostWithNewAuthorShouldCreateNewPost()
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
        public async Task CreateNewPostWithNewAuthorShouldStoreBothEvents()
        {
            //var authorRequest = CreateAuthorRequestBuilder.Default().Build();
            //var postRequest = CreatePostRequestBuilder.Default().WithAuthor(authorRequest).Build();

            //var response = await _client.PostAsJsonAsync("/post", postRequest);
            //response.StatusCode.Should().Be(HttpStatusCode.Created);

            //var postId = await response.Content.ReadFromJsonAsync<Guid>();

            //await _client.WaitForProjectionAsync<PostDto>(
            //    url: $"/post/{postId}",
            //    predicate: p => p is not null,
            //    timeoutSeconds: 10);

            //await using var scope = _factory.Services.CreateAsyncScope();
            //var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            //var events = await db.Set<StoredEvent>()
            //    .AsNoTracking()
            //    .ToListAsync();

            //events.Should().Contain(e => e.EventType.Contains("PostCreatedEvent"));
            //events.Should().Contain(e => e.EventType.Contains("AuthorCreatedEvent"));
        }

        //[Fact]
        //public async Task CreateNewPostWithExistingAuthorShouldStorePostEvent()
        //{
        //    var authorRequest = CreateAuthorRequestBuilder.Default().Build();

        //    var postRequest1 = CreatePostRequestBuilder.Default()
        //        .WithAuthor(authorRequest)
        //        .Build();

        //    var response1 = await _client.PostAsJsonAsync("/post", postRequest1);
        //    response1.StatusCode.Should().Be(HttpStatusCode.Created);

        //    var postId1 = await response1.Content.ReadFromJsonAsync<Guid>();
        //    postId1.Should().NotBeEmpty();

        //    var getResponse = await _client.GetAsync($"/post/{postId1}?includeAuthor=true");
        //    getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        //Guid createdAuthorId;

        //await using (var scope = _factory.Services.CreateAsyncScope())
        //{
        //    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        //    var events = await db.Set<StoredEvent>()
        //        .AsNoTracking()
        //        .ToListAsync();

        //    createdAuthorId = events
        //        .Where(e => e.StreamId.StartsWith("author-"))
        //        .Select(e => Guid.Parse(e.StreamId["author-".Length..]))
        //        .Single();
        //}

        //var postRequest2 = CreatePostRequestBuilder.Default()
        //    .WithAuthorId(createdAuthorId)
        //    .Build();

        //var response2 = await _client.PostAsJsonAsync("/post", postRequest2);
        //response2.StatusCode.Should().Be(HttpStatusCode.Created);

        //await using (var scope = _factory.Services.CreateAsyncScope())
        //{
        //    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        //    var events = await db.Set<StoredEvent>()
        //        .AsNoTracking()
        //        .ToListAsync();

        //    events.Count(e => e.EventType == "PostCreatedEvent").Should().Be(2);
        //    events.Count(e => e.EventType == "AuthorCreatedEvent").Should().Be(1);
        //}
    }
}

