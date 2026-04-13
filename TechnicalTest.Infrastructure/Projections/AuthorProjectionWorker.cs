using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TechnicalTest.Application.Abstractions.Events;
using TechnicalTest.Domain.Events;
using TechnicalTest.Domain.Models;
using TechnicalTest.Infrastructure.Models;

namespace TechnicalTest.Infrastructure.Projections
{
    public class AuthorProjectionWorker(IServiceScopeFactory scopeFactory) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var eventStore = scope.ServiceProvider.GetRequiredService<IEventStore>();

                    var checkpoint = await db.ProjectionCheckpoints
                        .FirstOrDefaultAsync(x => x.Name == "AuthorProjection", stoppingToken);

                    if (checkpoint is null)
                    {
                        checkpoint = new ProjectionCheckpoint
                        {
                            Name = "AuthorProjection",
                            LastProcessedEventId = 0
                        };

                        db.ProjectionCheckpoints.Add(checkpoint);
                        await db.SaveChangesAsync(stoppingToken);
                    }

                    var batch = await eventStore.LoadAllAsync(
                        checkpoint.LastProcessedEventId,
                        take: 100,
                        stoppingToken
                    );

                    if (batch.Count == 0)
                    {
                        await Task.Delay(300, stoppingToken);
                        continue;
                    }

                    foreach (var envelope in batch)
                    {
                        if (envelope.Event is AuthorCreatedEvent created)
                        {
                            var exists = await db.Authors
                                .AnyAsync(x => x.Id == created.AuthorId, stoppingToken);

                            if (!exists)
                            {
                                db.Authors.Add(new AuthorReadModel
                                {
                                    Id = created.AuthorId,
                                    Name = created.Name,
                                    Surname = created.Surname
                                });
                            }
                        }

                        checkpoint.LastProcessedEventId = envelope.Id;
                    }

                    await db.SaveChangesAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex) when (ex.InnerException is OperationCanceledException)
                {
                    break;
                }
            }
        }
    }
}
