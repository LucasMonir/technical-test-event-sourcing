using TechnicalTest.Application.Abstractions.Persistence;

namespace TechnicalTest.Infrastructure.Services
{
    internal sealed class UnitOfWork(AppDbContext db) : IUnitOfWork
    {
        public Task CommitAsync(CancellationToken cancellationToken = default)
            => db.SaveChangesAsync(cancellationToken);
    }
}
