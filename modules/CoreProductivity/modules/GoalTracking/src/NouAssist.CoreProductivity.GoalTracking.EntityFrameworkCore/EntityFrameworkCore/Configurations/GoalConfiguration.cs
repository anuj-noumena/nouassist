using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.CoreProductivity.GoalTracking.Aggregates;
using NouAssist.CoreProductivity.GoalTracking.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.CoreProductivity.GoalTracking.EntityFrameworkCore.Configurations
{
    public class GoalConfiguration : IEntityTypeConfiguration<Goal>
    {
        public void Configure(EntityTypeBuilder<Goal> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(GoalsDbProperties.DbTablePrefix + "Goals",
                            GoalsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Description)
                .HasMaxLength(5000);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Progress)
                .IsRequired();

            builder.Property(x => x.SmartDescription)
                .HasMaxLength(2000);

            builder.Property(x => x.CompletionNotes)
                .HasMaxLength(2000);

            builder.Property(x => x.ArchiveReason)
                .HasMaxLength(500);

            // Complex property for ImportanceScore
            builder.OwnsOne(x => x.Importance, ib =>
            {
                ib.Property(i => i.Value)
                    .HasColumnName("ImportanceScore")
                    .IsRequired()
                    .HasPrecision(3, 2);
            });

            // Complex property for UrgencyScore
            builder.OwnsOne(x => x.Urgency, ub =>
            {
                ub.Property(u => u.Value)
                    .HasColumnName("UrgencyScore")
                    .IsRequired()
                    .HasPrecision(3, 2);
            });

            builder.Property(x => x.TargetDate);
            builder.Property(x => x.CompletedAt);

            // One-to-many relationship with GoalMilestone
            builder.HasMany(x => x.Milestones)
                .WithOne()
                .HasForeignKey(m => m.GoalId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.OwnerId, x.Status });
            builder.HasIndex(x => x.AreaId);
            builder.HasIndex(x => x.TargetDate);
            builder.HasIndex(x => x.Progress);

            builder.ApplyObjectExtensionMappings();
        }
    }
}
