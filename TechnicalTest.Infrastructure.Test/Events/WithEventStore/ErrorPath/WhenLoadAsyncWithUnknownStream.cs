using FluentAssertions;
using TechnicalTest.Infrastructure.Events;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Test.Events.WithEventStore.ErrorPath
{
    public sealed class WhenLoadAsyncWithUnknownStream : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly EfEventStore _sut;

        private readonly string _streamId;

        public WhenLoadAsyncWithUnknownStream()
        {
            _dbContext = new TestDbContextFactory().Context;
            _sut = new EfEventStore(_dbContext);

            _streamId = Guid.NewGuid().ToString();
        }

        [Fact]
        public async Task ThenMustReturnEmptyEventsAndVersionZero()
        {
            var result = await _sut.LoadAsync(_streamId);

            result.Events.Should().BeEmpty();
            result.Version.Should().Be(0);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
    }
}
