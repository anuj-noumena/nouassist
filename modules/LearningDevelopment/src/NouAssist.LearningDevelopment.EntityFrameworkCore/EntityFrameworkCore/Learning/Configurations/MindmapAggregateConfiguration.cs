using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.Learning.Configurations
{
    public class MindmapAggregateConfiguration : IEntityTypeConfiguration<Mindmap>
    {
        public void Configure(EntityTypeBuilder<Mindmap> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                LearningDbProperties.DbTablePrefix + "Mindmaps",
                LearningDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.UserId)
                .IsRequired()
                .HasColumnName(nameof(Mindmap.UserId));

            builder.Property(x => x.TopicId)
                .IsRequired()
                .HasColumnName(nameof(Mindmap.TopicId));

            builder.Property(x => x.Url)
                .HasMaxLength(2048)
                .HasColumnName(nameof(Mindmap.Url));

            // Complex Type: Data (stored as JSON - nodes and edges)
            builder.Property("_data")
                .HasColumnName("Data")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null));

            // Indexes
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_Learning_Mindmaps_UserId");

            builder.HasIndex(x => x.TopicId)
                .IsUnique()
                .HasDatabaseName("IX_Learning_Mindmaps_TopicId");

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(Mindmap.ExtraProperties))
                .HasColumnType("jsonb");

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40)
                .HasColumnName(nameof(Mindmap.ConcurrencyStamp));
        }
    }
}
