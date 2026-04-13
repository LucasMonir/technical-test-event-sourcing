using FluentAssertions;
using NSubstitute;
using TechnicalTest.Application.Abstractions.Events;
using TechnicalTest.Application.Abstractions.Services;
using TechnicalTest.Application.Commands;
using TechnicalTest.Application.Services;
using TechnicalTest.Domain.Events.Base;
using TechnicalTest.TestHelpers.Builders.Application;

namespace TechnicalTest.Application.Test.Services.WithPostCommandHandler.ErrorPath
{
    public sealed class WhenHandleCreatePostCommandAuthorResolverFails
    {
        private readonly IAuthorResolver _authorResolver;
        private readonly IEventStore _eventStore;
        private readonly PostCommandHandler _sut;

        private readonly CreatePostCommand _command;

        public WhenHandleCreatePostCommandAuthorResolverFails()
        {
            _command = CreatePostCommandBuilder.Default()
             .Build();

            _authorResolver = Substitute.For<IAuthorResolver>();
            _authorResolver.ResolveAsync(_command)
              .Returns<Task<Guid>>(_ => throw new InvalidOperationException("Author invalid"));

            _eventStore = Substitute.For<IEventStore>();

            _sut = new PostCommandHandler(_authorResolver, _eventStore);
        }

        [Fact]
        public async Task ThenMustThrowAndNotAppend()
        {
            var act = async () => await _sut.Handle(_command);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Author invalid");

            await _eventStore.DidNotReceive()
                .AppendAsync(Arg.Any<string>(),
                    Arg.Any<int>(),
                    Arg.Any<IReadOnlyCollection<IDomainEvent>>()
                );
        }
    }
}
