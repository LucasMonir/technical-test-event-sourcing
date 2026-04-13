using FluentAssertions;
using TechnicalTest.Infrastructure.Events;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Test.Events.WithEventStore.ErrorPath
{
    public sealed class WhenLoadAllAsyncWithUnknownEventType : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly EfEventStore _sut;

        public WhenLoadAllAsyncWithUnknownEventType()
        {
            _dbContext = new TestDbContextFactory().Context;
            _sut = new EfEventStore(_dbContext);
        }

        [Fact]
        public async Task ThenMustThrowUnknownEventType()
        {
            var act = async () => await _sut.LoadAllAsync(afterId: 0, take: 100);

            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Unknown event type: WeirdEvent");
        }


        #region Setup/Teardown
        public async Task InitializeAsync()
        {
            _dbContext.StoredEvents.Add(new StoredEvent
            {
                StreamId = Guid.NewGuid().ToString(),
                Version = 1,
                EventType = "WeirdEvent",
                EventData = "{}",
                OccurredAt = DateTimeOffset.UtcNow
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
        #endregion
    }
}
