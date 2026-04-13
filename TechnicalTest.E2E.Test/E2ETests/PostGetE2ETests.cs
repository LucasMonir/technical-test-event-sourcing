using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using TechnicalTest.Application.Abstractions.Events;
using TechnicalTest.Application.DTOs;
using TechnicalTest.Domain.Events;
using TechnicalTest.Infrastructure;
using TechnicalTest.TestHelpers.Builders.Domain;

namespace TechnicalTest.E2E.Test.E2ETests
{
    [Collection("E2E")]
    public class PostGetE2ETests : IClassFixture<TechnicalTestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly AppDbContext _dbContext;
        private readonly IEventStore _eventStore;

        public PostGetE2ETests(TechnicalTestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();

            var scope = factory.Services.CreateScope();
            _eventStore = scope.ServiceProvider.GetRequiredService<IEventStore>();
            _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        }


        [Fact]
        public async Task GetPostIncludingAuthorReturnsPostWithAuthor()
        {
            var author = AuthorBuilder.Default().Build();

            var authorCreated = new AuthorCreatedEvent(
                author.Id,
                author.Name,
                author.Surname,
                DateTimeOffset.UtcNow
            );

            await _eventStore.AppendAsync(
                $"author-{author.Id}",
                expectedVersion: 0,
                [authorCreated]
            );

            var post = PostBuilder.Default()
                .WithAutorId(author.Id)
                .Build();

            var postCreated = new PostCreatedEvent(
                post.Id,
                post.AuthorId,
                post.Title,
                post.Description,
                post.Content,
                DateTimeOffset.UtcNow
            );

            await _eventStore.AppendAsync(
                $"post-{post.Id}",
                expectedVersion: 0,
                new[] { postCreated }
            );

            await WaitForPostProjection(post.Id);

            var response = await _client.GetAsync($"/post/{post.Id}?includeAuthor=true");

            var result = await response.Content.ReadFromJsonAsync<PostDto>();

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(
                new PostDto(
                    post.Id,
                    author.Id,
                    post.Title,
                    post.Description,
                    post.Content,
                    new AuthorDto(author.Id, author.Name, author.Surname)
                )
            );
        }

        [Fact]
        public async Task GetPostIncludingAuthorReturnsPostWithoutAuthor()
        {
            var author = AuthorBuilder.Default().Build();

            var authorCreated = new AuthorCreatedEvent(
                author.Id,
                author.Name,
                author.Surname,
                DateTimeOffset.UtcNow
            );

            await _eventStore.AppendAsync(
                $"author-{author.Id}",
                expectedVersion: 0,
                [authorCreated]
            );

            var post = PostBuilder.Default()
                .WithAutorId(author.Id)
                .Build();

            var postCreated = new PostCreatedEvent(
                post.Id,
                post.AuthorId,
                post.Title,
                post.Description,
                post.Content,
                DateTimeOffset.UtcNow
            );

            await _eventStore.AppendAsync(
                $"post-{post.Id}",
                expectedVersion: 0,
                [postCreated]
            );

            var response = await _client.GetAsync($"/post/{post.Id}");

            var result = await response.Content.ReadFromJsonAsync<PostDto>();

            result.Should().NotBeNull();
            result!.Author.Should().BeNull();

            result.Should().BeEquivalentTo(
                new PostDto(
                    post.Id,
                    author.Id,
                    post.Title,
                    post.Description,
                    post.Content,
                    null
                ),
                options => options.Excluding(p => p.Author)
            );
        }

        private async Task WaitForPostProjection(Guid postId)
        {
            var timeout = DateTime.UtcNow.AddSeconds(5);

            while (DateTime.UtcNow < timeout)
            {
                var response = await _client.GetAsync($"/post/{postId}?includeAuthor=true");

                if (response.IsSuccessStatusCode)
                    return;

                await Task.Delay(50);
            }

            throw new Exception("Projection did not complete in time");
        }
    }
}
