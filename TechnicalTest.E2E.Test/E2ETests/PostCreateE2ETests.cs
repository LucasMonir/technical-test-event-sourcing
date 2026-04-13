using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using TechnicalTest.Infrastructure;
using TechnicalTest.Infrastructure.Events;
using TechnicalTest.TestHelpers.Builders.Application;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace TechnicalTest.E2E.Test.E2ETests
{
    public class PostCreateE2ETests : IClassFixture<TechnicalTestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly TechnicalTestWebApplicationFactory _factory;
        private readonly int _eventVersion = 1;

        public PostCreateE2ETests(TechnicalTestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateNewPostWithNewAuthorShouldStoreBothEvents()
        {
            var authorRequest = CreateAuthorRequestBuilder.Default()
                .Build();

            var postRequest = CreatePostRequestBuilder.Default()
                .WithAuthor(authorRequest)
                .Build();

            var response = await _client.PostAsJsonAsync($"/post", postRequest);
            var result = await response.Content.ReadFromJsonAsync<Guid>();

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            result.Should().NotBeEmpty();

            await using var scope = _factory.Services.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var events = await db.Set<StoredEvent>()
                .AsNoTracking()
                .ToListAsync();

            events.Should().NotBeEmpty();

            events.Should().Contain(e =>
                e.EventType.Contains("PostCreatedEvent"));

            events.Should().Contain(e =>
                e.EventType.Contains("AuthorCreatedEvent"));
        }

        [Fact]
        public async Task CreateNewPostWithExistingAuthorShouldStorePostEvent()
        {

            var authorRequest = CreateAuthorRequestBuilder.Default().Build();

            var postRequest1 = CreatePostRequestBuilder.Default()
                .WithAuthor(authorRequest)
                .Build();

            var response1 = await _client.PostAsJsonAsync("/post", postRequest1);
            response1.StatusCode.Should().Be(HttpStatusCode.Created);

            var postId1 = await response1.Content.ReadFromJsonAsync<Guid>();
            postId1.Should().NotBeEmpty();

            Guid authorId;

            //await using (var scope = _factory.Services.CreateAsyncScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            //    authorId = await db.Authors
            //        .AsNoTracking()
            //        .Select(a => a.Id)
            //        .FirstAsync();
            //}

            //var postRequest2 = CreatePostRequestBuilder.Default()
            //    .WithAuthorId(authorId)
            //    .Build();

            //var response2 = await _client.PostAsJsonAsync("/post", postRequest2);
            //response2.StatusCode.Should().Be(HttpStatusCode.Created);

            //var postId2 = await response2.Content.ReadFromJsonAsync<Guid>();
            //postId2.Should().NotBeEmpty();

            //await using (var scope = _factory.Services.CreateAsyncScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            //    var events = await db.Set<StoredEvent>()
            //        .AsNoTracking()
            //        .ToListAsync();

            //    events.Count(e => e.EventType.Contains("PostCreatedEvent"))
            //        .Should().Be(2);

            //    events.Count(e => e.EventType.Contains("AuthorCreatedEvent"))
            //        .Should().Be(1);

            //}
        }
    }
}
