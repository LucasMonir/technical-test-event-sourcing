using FluentAssertions;
using TechnicalTest.Infrastructure.Events;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Test.Events.WithEventStore.ErrorPath
{
    public class WhenLoadAsyncWithInvalidStreamId : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly EfEventStore _sut;

        public WhenLoadAsyncWithInvalidStreamId()
        {
            _dbContext = new TestDbContextFactory().Context;
            _sut = new EfEventStore(_dbContext);
        }

        [Fact]
        public async Task ThenMustThrowArgumentException_WhenStreamIdIsEmpty()
        {
            var act = async () => await _sut.LoadAsync(streamId: string.Empty);

            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task ThenMustThrowArgumentException_WhenStreamIdIsWhitespace()
        {
            var act = async () => await _sut.LoadAsync(streamId: " ");

            await act.Should().ThrowAsync<ArgumentException>();
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
    }
}
