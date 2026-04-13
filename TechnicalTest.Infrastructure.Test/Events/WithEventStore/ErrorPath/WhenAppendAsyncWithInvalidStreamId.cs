using FluentAssertions;
using TechnicalTest.Domain.Events;
using TechnicalTest.Infrastructure.Events;
using TechnicalTest.TestHelpers.Builders.Application;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Test.Events.WithEventStore.ErrorPath
{
    public class WhenAppendAsyncWithInvalidStreamId : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly EfEventStore _sut;
        private readonly PostCreatedEvent _event;

        public WhenAppendAsyncWithInvalidStreamId()
        {
            _dbContext = new TestDbContextFactory().Context;
            _sut = new EfEventStore(_dbContext);

            _event = PostCreatedEventBuilder.Default().Build();
        }

        [Fact]
        public async Task ThenMustThrowArgumentExceptionWhenStreamIdIsEmpty()
        {
            var act = async () => await _sut.AppendAsync(
                streamId: "",
                expectedVersion: 0,
                events: [_event]);

            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task ThenMustThrowArgumentExceptionWhenStreamIdIsWhitespace()
        {
            var act = async () => await _sut.AppendAsync(
                streamId: "   ",
                expectedVersion: 0,
                events: [_event]);

            await act.Should().ThrowAsync<ArgumentException>();
        }


        #region Setup/Teardown
        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
        #endregion  
    }
}
