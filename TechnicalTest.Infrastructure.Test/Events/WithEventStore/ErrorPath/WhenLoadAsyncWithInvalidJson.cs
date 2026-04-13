using FluentAssertions;
using TechnicalTest.Domain.Events;
using TechnicalTest.Infrastructure.Events;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Test.Events.WithEventStore.ErrorPath
{
    public class WhenLoadAsyncWithInvalidJson : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly EfEventStore _sut;

        private readonly string _streamId;

        public WhenLoadAsyncWithInvalidJson()
        {
            _dbContext = new TestDbContextFactory().Context;
            _sut = new EfEventStore(_dbContext);

            _streamId = Guid.NewGuid().ToString();
        }

        [Fact]
        public async Task ThenMustThrowJsonException()
        {
            var act = async () => await _sut.LoadAsync(_streamId);

            await act.Should().ThrowAsync<System.Text.Json.JsonException>();
        }

        public async Task InitializeAsync()
        {
            _dbContext.StoredEvents.Add(new StoredEvent
            {
                StreamId = _streamId,
                Version = 1,
                EventType = nameof(PostCreatedEvent),
                EventData = "{ invalid json }",
                OccurredAt = DateTimeOffset.UtcNow
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
    }
}
