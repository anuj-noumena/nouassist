using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.Reviews.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.Reviews.Configurations
{
    public class ReviewTemplateConfiguration : IEntityTypeConfiguration<ReviewTemplate>
    {
        public void Configure(EntityTypeBuilder<ReviewTemplate> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ReviewsDbProperties.DbTablePrefix + "ReviewTemplates",
                           ReviewsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ReviewType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.IsSystem)
                .IsRequired();

            builder.Property(x => x.OwnerId);

            builder.Property(x => x.RecommendedFrequency)
                .HasMaxLength(100);

            builder.Property(x => x.EstimatedMinutes);

            // Checklist Items as JSON
            builder.OwnsMany(x => x.ChecklistItems, checklistBuilder =>
            {
                checklistBuilder.ToJson();
                checklistBuilder.Property(c => c.Step).IsRequired();
                checklistBuilder.Property(c => c.Title).IsRequired().HasMaxLength(200);
                checklistBuilder.Property(c => c.Description).HasMaxLength(1000);
                checklistBuilder.Property(c => c.Items).IsRequired();
            });

            // Reflection Prompts as JSON
            builder.Property(x => x.ReflectionPrompts)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null) ?? new List<string>()
                )
                .HasColumnType("jsonb");

            builder.HasIndex(x => x.ReviewType);
            builder.HasIndex(x => x.IsSystem);
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.TenantId);

            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
