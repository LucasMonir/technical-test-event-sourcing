using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechnicalTest.Domain.Models;

namespace TechnicalTest.Infrastructure.Configurations
{
    internal class ProjectionCheckpointConfiguration : IEntityTypeConfiguration<ProjectionCheckpoint>
    {
        public void Configure(EntityTypeBuilder<ProjectionCheckpoint> builder)
        {
            builder.HasKey(x => x.Name);
            builder.Property(x => x.LastProcessedEventId).IsRequired();
        }
    }
}