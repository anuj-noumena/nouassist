using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.PersonalDevelopment.Goals;
using NouAssist.PersonalDevelopment.Goals.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.PersonalDevelopment.EntityFrameworkCore.Goals.Configurations
{
    public class GoalMilestoneConfiguration : IEntityTypeConfiguration<GoalMilestone>
    {
        public void Configure(EntityTypeBuilder<GoalMilestone> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(PersonalDevelopmentDbProperties.DbTablePrefix + "GoalMilestones",
                PersonalDevelopmentDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.GoalId)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.TargetDate);

            builder.Property(x => x.IsCompleted)
                .IsRequired();

            builder.Property(x => x.CompletedAt);

            builder.Property(x => x.OrderIndex)
                .IsRequired();

            builder.HasIndex(x => x.GoalId);
            builder.HasIndex(x => new { x.GoalId, x.OrderIndex });

            builder.ApplyObjectExtensionMappings();
        }
    }
}
