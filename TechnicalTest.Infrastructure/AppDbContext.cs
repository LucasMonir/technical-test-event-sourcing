using Microsoft.EntityFrameworkCore;
using TechnicalTest.Domain;
using TechnicalTest.Infrastructure.Configurations;
using TechnicalTest.Infrastructure.Events;

namespace TechnicalTest.Infrastructure
{
    internal sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<StoredEvent> StoredEvents => Set<StoredEvent>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new StoredEventConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
