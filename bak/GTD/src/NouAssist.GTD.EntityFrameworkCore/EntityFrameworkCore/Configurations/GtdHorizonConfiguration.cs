using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.GTD.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.GTD.EntityFrameworkCore.Configurations
{
    public class GtdHorizonConfiguration : IEntityTypeConfiguration<GtdHorizon>
    {
        public void Configure(EntityTypeBuilder<GtdHorizon> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(GtdDbProperties.DbTablePrefix + "Horizons",
                            GtdDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.HorizonLevel)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Description)
                .HasMaxLength(5000);

            builder.Property(x => x.Content)
                .HasMaxLength(10000);

            builder.Property(x => x.LifePurpose)
                .HasMaxLength(5000);

            builder.Property(x => x.CoreValues)
                .HasColumnType("jsonb");

            builder.Property(x => x.VisionStatement)
                .HasMaxLength(5000);

            builder.Property(x => x.KeyThemes)
                .HasColumnType("jsonb");

            builder.Property(x => x.RelatedGoalIds)
                .HasColumnType("jsonb");

            builder.Property(x => x.RelatedAreaIds)
                .HasColumnType("jsonb");

            builder.Property(x => x.RelatedProjectIds)
                .HasColumnType("jsonb");

            builder.Property(x => x.ReviewFrequency)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.LastReviewedAt);
            builder.Property(x => x.NextReviewAt);
            builder.Property(x => x.ReviewCount).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsDraft).IsRequired();
            builder.Property(x => x.Version).IsRequired();

            builder.HasIndex(x => new { x.OwnerId, x.HorizonLevel });
            builder.HasIndex(x => x.NextReviewAt);

            builder.ApplyObjectExtensionMappings();
        }
    }
}
