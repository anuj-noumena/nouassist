using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore.Configurations
{
    public class BusinessModelCanvasAggregateConfiguration : IEntityTypeConfiguration<BusinessModelCanvas>
    {
        public void Configure(EntityTypeBuilder<BusinessModelCanvas> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(BusinessModelManagementDbProperties.DbTablePrefix + "BusinessModelCanvases",
                           BusinessModelManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(BusinessModelManagementConsts.BusinessModelCanvas.MaxTitleLength);

            builder.Property(x => x.Description)
                .HasMaxLength(BusinessModelManagementConsts.BusinessModelCanvas.MaxDescriptionLength);

            builder.Property(x => x.CurrentVersionNumber)
                .IsRequired();

            builder.Property(x => x.CurrentVersionId);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            // Relationships
            builder.HasMany(x => x.Versions)
                .WithOne()
                .HasForeignKey("CanvasId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Components)
                .WithOne()
                .HasForeignKey("CanvasId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.TenantId);
            builder.HasIndex(x => new { x.TenantId, x.Title });

            // Multi-tenancy
            builder.Property(x => x.TenantId);

            // Extra Properties
            builder.Property(x => x.ExtraProperties).HasColumnName(nameof(BusinessModelCanvas.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
