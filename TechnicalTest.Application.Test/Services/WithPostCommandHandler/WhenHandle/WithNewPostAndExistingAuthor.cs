using FluentAssertions;
using NSubstitute;
using TechnicalTest.Application.Abstractions.Events;
using TechnicalTest.Application.Abstractions.Services;
using TechnicalTest.Application.Commands;
using TechnicalTest.Application.Services;
using TechnicalTest.Domain.Events.Base;
using TechnicalTest.TestHelpers.Builders.Application;

namespace TechnicalTest.Application.Test.Services.WithPostCommandHandler.WhenHandle
{
    public class WithNewPostAndExistingAuthor
    {
        private readonly IAuthorResolver _authorResolver;
        private readonly IEventStore _eventStore;

        private readonly PostCommandHandler _sut;
        private readonly CreatePostCommand _createPostCommand;
        private readonly Guid _authorId;

        public WithNewPostAndExistingAuthor()
        {
            _authorResolver = Substitute.For<IAuthorResolver>();
            _eventStore = Substitute.For<IEventStore>();

            _authorId = Guid.NewGuid();

            _createPostCommand = CreatePostCommandBuilder.Default()
                .WithAuthorId(_authorId)
                .Build();

            _authorResolver.ResolveAsync(_createPostCommand)
                .Returns(_authorId);

            _sut = new(_authorResolver, _eventStore);
        }

        [Fact]
        public async Task ThenMustReturnNewPostId()
        {
            var result = await _sut.Handle(_createPostCommand);

            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ThenMustCallAuthorResolver()
        {
            _ = await _sut.Handle(_createPostCommand);

            await _authorResolver.Received()
                .ResolveAsync(_createPostCommand);
        }


        [Fact]
        public async Task ThenMustCallEventStore()
        {
            var result = await _sut.Handle(_createPostCommand);

            await _eventStore.Received()
                .AppendAsync(
                    Arg.Is($"post-{result}"),
                    Arg.Any<int>(),
                    Arg.Any<IReadOnlyCollection<IDomainEvent>>(),
                    Arg.Any<CancellationToken>()
                );
        }
    }
}
