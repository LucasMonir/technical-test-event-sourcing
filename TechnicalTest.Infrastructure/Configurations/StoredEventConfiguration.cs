using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechnicalTest.Infrastructure.Persistence.Events;

namespace TechnicalTest.Infrastructure.Persistence.Configurations
{
    internal class StoredEventConfiguration : IEntityTypeConfiguration<StoredEvent>
    {
        public void Configure(EntityTypeBuilder<StoredEvent> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.StreamId).IsRequired();
            builder.Property(x => x.Version).IsRequired();
            builder.Property(x => x.EventType).IsRequired();
            builder.Property(x => x.EventData).IsRequired();
            builder.Property(x => x.OccurredAt).IsRequired();

            builder.HasIndex(x => new { x.StreamId, x.Version }).IsUnique();
        }
    }
}
