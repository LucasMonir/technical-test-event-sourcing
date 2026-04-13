using FluentAssertions;
using TechnicalTest.Domain.Events;
using TechnicalTest.Infrastructure.Events;
using TechnicalTest.TestHelpers.Builders.Application;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Test.Events.WithEventStore.ErrorPath
{

    public sealed class WhenAppendAsyncWithWrongExpectedVersion : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly EfEventStore _sut;

        private readonly string _streamId;
        private readonly PostCreatedEvent _event;

        public WhenAppendAsyncWithWrongExpectedVersion()
        {
            _dbContext = new TestDbContextFactory().Context;
            _sut = new EfEventStore(_dbContext);

            _streamId = Guid.NewGuid().ToString();
            _event = PostCreatedEventBuilder.Default()
                .Build();
        }

        [Fact]
        public async Task ThenMustThrowConcurrencyConflict()
        {
            var act = async () => await _sut.AppendAsync(
                _streamId,
                expectedVersion: 0,
                [_event]);

            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage($"Concurrency conflict on {_streamId}. Expected 0, got 1.");
        }

        public async Task InitializeAsync()
        {
            await _sut.AppendAsync(_streamId, 0, [_event]);
        }

        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
    }

}