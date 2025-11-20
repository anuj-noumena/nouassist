using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore.Configurations
{
    public class FeedbackRecordConfiguration : IEntityTypeConfiguration<FeedbackRecord>
    {
        public void Configure(EntityTypeBuilder<FeedbackRecord> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                FundraisingPipelineDbProperties.DbTablePrefix + "FeedbackRecords", 
                FundraisingPipelineDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.PitchDeckId)
                .IsRequired();

            builder.Property(x => x.PresentationId)
                .IsRequired();

            builder.Property(x => x.Notes)
                .HasMaxLength(2000);

            // Value Object Mapping - FeedbackData
            builder.OwnsOne(x => x.Feedback, feedbackBuilder =>
            {
                feedbackBuilder.Property(f => f.Sentiment)
                    .HasColumnName("Sentiment")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                feedbackBuilder.Property(f => f.Rating)
                    .HasColumnName("Rating")
                    .IsRequired();

                // For the dictionary and list properties, we'll use JSON serialization
                feedbackBuilder.Property(f => f.CategoryFeedback)
                    .HasColumnName("CategoryFeedback")
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null) 
                            ?? new Dictionary<string, string>())
                    .HasColumnType("jsonb");

                feedbackBuilder.Property(f => f.StrengthPoints)
                    .HasColumnName("StrengthPoints")
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) 
                            ?? new List<string>())
                    .HasColumnType("jsonb");

                feedbackBuilder.Property(f => f.ImprovementAreas)
                    .HasColumnName("ImprovementAreas")
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) 
                            ?? new List<string>())
                    .HasColumnType("jsonb");
            });

            // Indexing
            builder.HasIndex(x => x.PitchDeckId);
            builder.HasIndex(x => x.PresentationId);
            builder.HasIndex("Sentiment"); // Index the owned property
            builder.HasIndex(x => x.CreationTime);
        }
    }
}
