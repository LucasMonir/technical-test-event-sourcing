using FluentAssertions;
using TechnicalTest.Application.Abstractions.Events;
using TechnicalTest.Domain.Events;
using TechnicalTest.Infrastructure.Events;
using TechnicalTest.TestHelpers.Builders.Application;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Test.Events.WithEventStore
{
    public class WhenLoadAsync : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly IEventStore _sut;

        private readonly string _streamId;
        private readonly int _expectedVersion;
        private readonly PostCreatedEvent _event;

        public WhenLoadAsync()
        {
            _streamId = Guid.NewGuid().ToString();
            _expectedVersion = 0;
            _event = PostCreatedEventBuilder.Default()
                .Build();

            _dbContext = new TestDbContextFactory().Context;
            _sut = new EfEventStore(_dbContext);
        }

        [Fact]
        public async Task ThenMustSaveEvent()
        {
            var result = await _sut.LoadAsync(_streamId);

            result.Events.Should().HaveCount(1);

            var @event = result.Events.First();

            var postCreated = @event.Should()
                .BeOfType<PostCreatedEvent>().Subject;

            postCreated.Should().BeEquivalentTo(_event, options =>
                options.Excluding(x => x.EventId));
        }

        #region Setup and Teardown
        public async Task InitializeAsync()
        {
            await _sut.AppendAsync(
               _streamId,
               _expectedVersion,
               [_event]);
        }
        async Task IAsyncLifetime.DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
        #endregion
    }
}
