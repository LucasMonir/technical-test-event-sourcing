using Microsoft.EntityFrameworkCore;
using TechnicalTest.Domain.Models;
using TechnicalTest.Infrastructure.Configurations;
using TechnicalTest.Infrastructure.Events;
using TechnicalTest.Infrastructure.Models;

namespace TechnicalTest.Infrastructure
{
    internal sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<StoredEvent> StoredEvents => Set<StoredEvent>();
        public DbSet<ProjectionCheckpoint> ProjectionCheckpoints => Set<ProjectionCheckpoint>();
        public DbSet<PostReadModel> Posts => Set<PostReadModel>();
        public DbSet<AuthorReadModel> Authors => Set<AuthorReadModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthorReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new PostReadModelConfiguration());
            modelBuilder.ApplyConfiguration(new StoredEventConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectionCheckpointConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
