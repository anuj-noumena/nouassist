using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Learning.Aggregates;
using NouAssist.Learning.Enums;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Learning.EntityFrameworkCore.Configurations
{
    public class LearningNoteAggregateConfiguration : IEntityTypeConfiguration<LearningNote>
    {
        public void Configure(EntityTypeBuilder<LearningNote> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                LearningDbProperties.DbTablePrefix + "LearningNotes",
                LearningDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.UserId)
                .IsRequired()
                .HasColumnName(nameof(LearningNote.UserId));

            builder.Property(x => x.TopicId)
                .IsRequired()
                .HasColumnName(nameof(LearningNote.TopicId));

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName(nameof(LearningNote.Type));

            // Cornell Note Fields
            builder.Property(x => x.Questions)
                .HasMaxLength(2000)
                .HasColumnName(nameof(LearningNote.Questions));

            builder.Property(x => x.Notes)
                .HasMaxLength(8000)
                .HasColumnName(nameof(LearningNote.Notes));

            builder.Property(x => x.Summary)
                .HasMaxLength(4000)
                .HasColumnName(nameof(LearningNote.Summary));

            // General Content
            builder.Property(x => x.Content)
                .HasColumnType("text")
                .HasColumnName(nameof(LearningNote.Content));

            // Complex Type: KeyPoints (stored as JSON)
            builder.Property("_keyPoints")
                .HasColumnName("KeyPoints")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.List<string>>(v, (System.Text.Json.JsonSerializerOptions)null));

            // Complex Type: Metadata (stored as JSON)
            builder.Property("_metadata")
                .HasColumnName("Metadata")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null));

            // Indexes
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_Learning_LearningNotes_UserId");

            builder.HasIndex(x => x.TopicId)
                .HasDatabaseName("IX_Learning_LearningNotes_TopicId");

            builder.HasIndex(x => new { x.UserId, x.Type })
                .HasDatabaseName("IX_Learning_LearningNotes_UserId_Type");

            builder.HasIndex(x => x.CreationTime)
                .HasDatabaseName("IX_Learning_LearningNotes_CreationTime");

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(LearningNote.ExtraProperties))
                .HasColumnType("jsonb");

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40)
                .HasColumnName(nameof(LearningNote.ConcurrencyStamp));
        }
    }
}
