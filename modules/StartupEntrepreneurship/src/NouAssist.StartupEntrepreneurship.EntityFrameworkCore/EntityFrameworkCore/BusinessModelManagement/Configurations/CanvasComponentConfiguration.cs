using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;
using System.Text.Json;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore.Configurations
{
    public class CanvasComponentConfiguration : IEntityTypeConfiguration<CanvasComponent>
    {
        public void Configure(EntityTypeBuilder<CanvasComponent> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(BusinessModelManagementDbProperties.DbTablePrefix + "CanvasComponents",
                           BusinessModelManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.CanvasId)
                .IsRequired();

            builder.Property(x => x.VersionId)
                .IsRequired();

            builder.Property(x => x.ComponentType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.LastModified)
                .IsRequired();

            builder.Property(x => x.LastModifiedBy)
                .IsRequired();

            // Value Object Configuration - ComponentContent
            builder.OwnsOne(x => x.Content, contentBuilder =>
            {
                contentBuilder.Property(c => c.Text)
                    .HasColumnName("ContentText")
                    .HasMaxLength(BusinessModelManagementConsts.BusinessModelCanvas.MaxComponentContentLength);

                contentBuilder.Property(c => c.BulletPoints)
                    .HasColumnName("ContentBulletPoints")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null));

                contentBuilder.Property(c => c.AdditionalProperties)
                    .HasColumnName("ContentAdditionalProperties")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null));
            });

            // Value Object Configuration - ValidationState
            builder.OwnsOne(x => x.ValidationState, validationBuilder =>
            {
                validationBuilder.Property(v => v.Status)
                    .HasColumnName("ValidationStatus")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                validationBuilder.Property(v => v.ConfidenceLevel)
                    .HasColumnName("ValidationConfidenceLevel")
                    .IsRequired();

                validationBuilder.Property(v => v.LastUpdated)
                    .HasColumnName("ValidationLastUpdated")
                    .IsRequired();

                validationBuilder.Property(v => v.Notes)
                    .HasColumnName("ValidationNotes")
                    .HasMaxLength(1000);
            });

            // Relationships
            builder.HasMany(x => x.Hypotheses)
                .WithOne()
                .HasForeignKey("ComponentId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.CanvasId);
            builder.HasIndex(x => x.VersionId);
            builder.HasIndex(x => new { x.CanvasId, x.VersionId, x.ComponentType }).IsUnique();
        }
    }
}
