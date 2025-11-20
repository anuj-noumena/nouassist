using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore.Configurations
{
    public class RevenueModelAggregateConfiguration : IEntityTypeConfiguration<RevenueModel>
    {
        public void Configure(EntityTypeBuilder<RevenueModel> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(BusinessModelManagementDbProperties.DbTablePrefix + "RevenueModels",
                           BusinessModelManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(BusinessModelManagementConsts.RevenueModel.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(BusinessModelManagementConsts.RevenueModel.MaxDescriptionLength);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            // Relationships
            builder.HasMany(x => x.PricingModels)
                .WithOne()
                .HasForeignKey("RevenueModelId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Scenarios)
                .WithOne()
                .HasForeignKey("RevenueModelId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.GtmStrategies)
                .WithOne()
                .HasForeignKey("RevenueModelId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.TenantId);
            builder.HasIndex(x => new { x.TenantId, x.Name });

            // Multi-tenancy
            builder.Property(x => x.TenantId);

            // Extra Properties
            builder.Property(x => x.ExtraProperties).HasColumnName(nameof(RevenueModel.ExtraProperties));

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
