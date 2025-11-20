using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.LearningDevelopment.Integration.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.Integration.Configurations
{
    public class LearningPathConfiguration : IEntityTypeConfiguration<LearningPath>
    {
        public void Configure(EntityTypeBuilder<LearningPath> builder)
        {
            builder.ToTable("LearningPaths");

            builder.ConfigureByConvention();

            // Properties
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.CurrentPhase)
                .IsRequired();

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.TopicId);
            builder.HasIndex(x => x.SkillId);
            builder.HasIndex(x => new { x.UserId, x.Status });
            builder.HasIndex(x => x.LastActivity);

            // Owned collections
            builder.Property(x => x.Milestones)
                .HasConversion(
                    v => string.Join("|", v),
                    v => v.Split("|", System.StringSplitOptions.RemoveEmptyEntries).ToList())
                .HasMaxLength(10000);
        }
    }
}
