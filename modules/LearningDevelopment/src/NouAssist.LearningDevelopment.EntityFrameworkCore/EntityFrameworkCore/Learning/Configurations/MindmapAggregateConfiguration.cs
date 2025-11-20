using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using System.Collections.Generic;
using System.Text.Json;
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
                LearningDevelopmentDbProperties.DbTablePrefix + "Mindmaps",
                LearningDevelopmentDbProperties.DbSchema);

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
            var dataConverter = new ValueConverter<Dictionary<string, object>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, (JsonSerializerOptions)null));

            builder.Property("_data")
                .HasColumnName("Data")
                .HasColumnType("jsonb")
                .HasConversion(dataConverter);

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
