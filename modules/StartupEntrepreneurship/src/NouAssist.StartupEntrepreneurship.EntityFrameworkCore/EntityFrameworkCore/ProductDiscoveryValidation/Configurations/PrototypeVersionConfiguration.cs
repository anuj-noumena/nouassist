using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Constants;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for PrototypeVersion entity
    /// </summary>
    public class PrototypeVersionConfiguration : IEntityTypeConfiguration<PrototypeVersion>
    {
        public void Configure(EntityTypeBuilder<PrototypeVersion> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(ProductDiscoveryDbProperties.DbTablePrefix + "PrototypeVersions", 
                           ProductDiscoveryDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.PrototypeId)
                .IsRequired();

            builder.Property(x => x.VersionNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(ProductDiscoveryConsts.MaxVersionNotesLength);

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Notes)
                .HasMaxLength(ProductDiscoveryConsts.MaxVersionNotesLength);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Configure PrototypeLink value object as owned type
            builder.OwnsOne(x => x.Link, linkBuilder =>
            {
                linkBuilder.Property(l => l.Url)
                    .HasColumnName("LinkUrl")
                    .HasMaxLength(ProductDiscoveryConsts.MaxUrlLength)
                    .IsRequired();

                linkBuilder.Property(l => l.Title)
                    .HasColumnName("LinkTitle")
                    .HasMaxLength(100)
                    .IsRequired();

                linkBuilder.Property(l => l.Type)
                    .HasColumnName("LinkType")
                    .HasMaxLength(50)
                    .IsRequired();

                linkBuilder.Property(l => l.AccessInstructions)
                    .HasColumnName("LinkAccessInstructions")
                    .HasMaxLength(500);
            });

            // Indexes
            builder.HasIndex(x => x.PrototypeId);
            builder.HasIndex(x => x.VersionNumber);
            builder.HasIndex(x => x.CreatedDate);
            builder.HasIndex(x => x.IsActive);

            // Unique constraint for version within prototype
            builder.HasIndex(x => new { x.PrototypeId, x.VersionNumber }).IsUnique();
        }
    }
}
