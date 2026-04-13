using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechnicalTest.Domain.Models;

namespace TechnicalTest.Infrastructure.Configurations
{
    internal class AuthorReadModelConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
               .IsRequired();

            builder.Property(x => x.Surname)
                .IsRequired();
        }
    }
}
