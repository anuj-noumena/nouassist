using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore.Configurations
{
    public class PitchVersionConfiguration : IEntityTypeConfiguration<PitchVersion>
    {
        public void Configure(EntityTypeBuilder<PitchVersion> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                FundraisingPipelineDbProperties.DbTablePrefix + "PitchVersions", 
                FundraisingPipelineDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.PitchDeckId)
                .IsRequired();

            builder.Property(x => x.VersionName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.VersionNumber)
                .IsRequired();

            builder.Property(x => x.ChangeNotes)
                .HasMaxLength(1000);

            // Value Object Mapping - DeckContent
            builder.OwnsOne(x => x.Content, contentBuilder =>
            {
                contentBuilder.Property(c => c.StorageUrl)
                    .HasColumnName("ContentStorageUrl")
                    .IsRequired()
                    .HasMaxLength(1000);

                contentBuilder.Property(c => c.ThumbnailUrl)
                    .HasColumnName("ContentThumbnailUrl")
                    .HasMaxLength(1000);

                contentBuilder.Property(c => c.SlideCount)
                    .HasColumnName("ContentSlideCount")
                    .IsRequired();

                contentBuilder.Property(c => c.ContentHash)
                    .HasColumnName("ContentHash")
                    .IsRequired()
                    .HasMaxLength(100);

                // For the SlideTopics collection, we'll use JSON serialization
                contentBuilder.Property(c => c.SlideTopics)
                    .HasColumnName("ContentSlideTopics")
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>())
                    .HasColumnType("jsonb");
            });

            // Indexing
            builder.HasIndex(x => x.PitchDeckId);
            builder.HasIndex(x => new { x.PitchDeckId, x.VersionNumber }).IsUnique();
            builder.HasIndex(x => x.CreationTime);
        }
    }
}
