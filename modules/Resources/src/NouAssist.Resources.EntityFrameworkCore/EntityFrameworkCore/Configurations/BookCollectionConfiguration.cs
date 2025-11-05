using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Resources.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Resources.EntityFrameworkCore.Configurations
{
    public class BookCollectionConfiguration : IEntityTypeConfiguration<BookCollection>
    {
        public void Configure(EntityTypeBuilder<BookCollection> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ResourcesDbProperties.DbTablePrefix + "BookCollections", 
                           ResourcesDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId).IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            // List columns
            builder.Property(x => x.Tags)
                .HasConversion(
                    v => string.Join(';', v),
                    v => v.Split(';', System.StringSplitOptions.RemoveEmptyEntries).ToList()
                )
                .HasMaxLength(1000);

            // Indexes
            builder.HasIndex(x => new { x.OwnerId, x.Name });
            builder.HasIndex(x => x.TenantId);
        }
    }
}
