using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TechnicalTest.Application.Abstractions.Events;
using TechnicalTest.Domain.Events;
using TechnicalTest.Infrastructure.Events;
using TechnicalTest.TestHelpers.Builders.Application;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Test.Events.WithEventStore
{
    public class WhenAppendingAsync : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly IEventStore _eventStore;

        private readonly string _streamId;
        private readonly int _expectedVersion;
        private readonly PostCreatedEvent _event;

        public WhenAppendingAsync()
        {
            _streamId = Guid.NewGuid().ToString();
            _expectedVersion = 0;
            _event = PostCreatedEventBuilder.Default()
                .Build();

            _dbContext = new TestDbContextFactory().Context;

            _eventStore = new EfEventStore(_dbContext);
        }

        [Fact]
        public async Task ThenMustSaveEvent()
        {
            await _eventStore.AppendAsync(
                _streamId,
                _expectedVersion,
                [_event]);

            var storedEvent = await _dbContext.StoredEvents
                .FirstOrDefaultAsync(x => x.StreamId == _streamId);

            storedEvent.Should().NotBeNull();

            storedEvent!.StreamId.Should().Be(_streamId);
            storedEvent.Version.Should().Be(1);
            storedEvent.EventType.Should().Be(typeof(PostCreatedEvent).Name);
        }
        #region Setup and Teardown
        public Task InitializeAsync() => Task.CompletedTask;
        async Task IAsyncLifetime.DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
        #endregion
    }
}
