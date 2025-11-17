using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.Learning.Configurations
{
    public class TopicAggregateConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                LearningDbProperties.DbTablePrefix + "Topics",
                LearningDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.UserId)
                .IsRequired()
                .HasColumnName(nameof(Topic.UserId));

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnName(nameof(Topic.Title));

            builder.Property(x => x.Description)
                .HasMaxLength(2000)
                .HasColumnName(nameof(Topic.Description));

            builder.Property(x => x.ParentId)
                .HasColumnName(nameof(Topic.ParentId));

            builder.Property(x => x.IsFolder)
                .IsRequired()
                .HasColumnName(nameof(Topic.IsFolder));

            builder.Property(x => x.Category)
                .HasMaxLength(128)
                .HasColumnName(nameof(Topic.Category));

            builder.Property(x => x.Difficulty)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName(nameof(Topic.Difficulty));

            builder.Property(x => x.EstimatedHours)
                .IsRequired()
                .HasColumnName(nameof(Topic.EstimatedHours));

            builder.Property(x => x.LearningGoals)
                .HasMaxLength(2000)
                .HasColumnName(nameof(Topic.LearningGoals));

            builder.Property(x => x.Progress)
                .IsRequired()
                .HasColumnName(nameof(Topic.Progress));

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasColumnName(nameof(Topic.Status));

            builder.Property(x => x.LearningSessionId)
                .HasColumnName(nameof(Topic.LearningSessionId));

            // Complex Type: Tags (stored as JSON array)
            builder.Property("_tags")
                .HasColumnName("Tags")
                .HasColumnType("jsonb") // PostgreSQL JSONB
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.List<string>>(v, (System.Text.Json.JsonSerializerOptions)null));

            // Self-referential relationship (Parent-Child)
            builder.HasOne<Topic>()
                .WithMany()
                .HasForeignKey(x => x.ParentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            // Owned collection: Children (mapped separately, loaded as part of aggregate)
            // Note: Children are loaded via explicit Include in repository
            // We don't use HasMany here to avoid automatic loading

            // Indexes
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_Learning_Topics_UserId");

            builder.HasIndex(x => x.ParentId)
                .HasDatabaseName("IX_Learning_Topics_ParentId");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_Learning_Topics_Status");

            builder.HasIndex(x => new { x.UserId, x.Status })
                .HasDatabaseName("IX_Learning_Topics_UserId_Status");

            builder.HasIndex(x => x.LearningSessionId)
                .IsUnique(false)
                .HasDatabaseName("IX_Learning_Topics_LearningSessionId");

            // Extra Properties (ABP handles JSON mapping by default)
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(Topic.ExtraProperties))
                .HasColumnType("jsonb");

            // Concurrency Stamp (Handled by ConfigureByConvention)
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40)
                .HasColumnName(nameof(Topic.ConcurrencyStamp));
        }
    }
}
