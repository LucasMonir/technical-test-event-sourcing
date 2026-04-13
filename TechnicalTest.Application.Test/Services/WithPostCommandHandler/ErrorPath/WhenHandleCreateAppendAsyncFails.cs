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
    public class WhenHandleCreateAppendAsyncFails
    {
        private readonly CreatePostCommand _command;
        private readonly IAuthorResolver _authorResolver;
        private readonly IEventStore _eventStore;
        private readonly PostCommandHandler _sut;

        public WhenHandleCreateAppendAsyncFails()
        {
            _command = CreatePostCommandBuilder.Default()
                .Build();

            _authorResolver = Substitute.For<IAuthorResolver>();
            _authorResolver.ResolveAsync(_command).Returns(Guid.NewGuid());

            _eventStore = Substitute.For<IEventStore>();
            _eventStore.LoadAsync(Arg.Any<string>())
                .Returns((Array.Empty<IDomainEvent>(), 0));

            _eventStore.AppendAsync(
                    Arg.Any<string>(),
                    Arg.Any<int>(),
                    Arg.Any<IReadOnlyCollection<IDomainEvent>>())
                .Returns(_ => throw new InvalidOperationException("Concurrency conflict"));

            _sut = new PostCommandHandler(_authorResolver, _eventStore);
        }

        [Fact]
        public async Task ThenMustThrowConcurrencyException()
        {
            var act = async () => await _sut.Handle(_command);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Concurrency conflict");
        }
    }
}
