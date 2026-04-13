using FluentAssertions;
using TechnicalTest.Domain.Events;
using TechnicalTest.Infrastructure.Events;
using TechnicalTest.TestHelpers.Builders.Application;
using TechnicalTest.TestHelpers.Database;

namespace TechnicalTest.Infrastructure.Test.Events.WithEventStore.HappyPath
{
    public class WhenLoadAllAsync : IAsyncLifetime
    {
        private readonly AppDbContext _dbContext;
        private readonly EfEventStore _sut;

        private readonly string _streamId1;
        private readonly string _streamId2;

        private readonly List<PostCreatedEvent> _events;

        public WhenLoadAllAsync()
        {
            _dbContext = new TestDbContextFactory().Context;
            _sut = new EfEventStore(_dbContext);

            _streamId1 = Guid.NewGuid().ToString();
            _streamId2 = Guid.NewGuid().ToString();

            _events =
            [
                GetNewPostEvent(),
                GetNewPostEvent(),
                GetNewPostEvent()
            ];
        }

        [Fact]
        public async Task ThenMustReturnEventsOrderedById()
        {
            var all = (await _sut.LoadAllAsync(afterId: 0, take: 100)).ToList();

            all.Should().HaveCount(3);
            all.Select(x => x.Id).Should().BeInAscendingOrder();
        }

        [Fact]
        public async Task ThenMustDeserializeAndReturnCorrectEvents()
        {
            var all = (await _sut.LoadAllAsync(afterId: 0, take: 100)).ToList();

            all.Select(x => (PostCreatedEvent)x.Event)
                .Should()
                .BeEquivalentTo(_events, o => o.Excluding(x => x.EventId));
        }

        [Fact]
        public async Task ThenMustFilterEventsAfterGivenId()
        {
            var all = (await _sut.LoadAllAsync(afterId: 0, take: 100)).ToList();

            var afterFirst = (await _sut.LoadAllAsync(afterId: all[0].Id, take: 100)).ToList();

            afterFirst.Should().HaveCount(2);
            afterFirst.Should().OnlyContain(x => x.Id > all[0].Id);
        }

        [Fact]
        public async Task ThenMustLimitReturnedEventsUsingTake()
        {
            var limited = (await _sut.LoadAllAsync(afterId: 0, take: 2)).ToList();

            limited.Should().HaveCount(2);
            limited.Select(x => x.Id).Should().BeInAscendingOrder();
        }

        private static PostCreatedEvent GetNewPostEvent() =>
            PostCreatedEventBuilder.Default().Build();

        #region Setup/Teardown
        public async Task InitializeAsync()
        {
            await _sut.AppendAsync(_streamId1, 0, [_events[0], _events[1]]);
            await _sut.AppendAsync(_streamId2, 0, [_events[2]]);
        }

        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
        #endregion
    }
}
