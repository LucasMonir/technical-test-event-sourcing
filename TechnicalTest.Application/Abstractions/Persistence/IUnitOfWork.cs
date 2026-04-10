namespace TechnicalTest.Application.Abstractions.Persistence
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}
