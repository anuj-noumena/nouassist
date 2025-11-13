using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.PersonalDevelopment.Habits.EntityFrameworkCore.Configurations
{
    public class RoutineCompletionConfiguration : IEntityTypeConfiguration<RoutineCompletion>
    {
        public void Configure(EntityTypeBuilder<RoutineCompletion> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(HabitsDbProperties.DbTablePrefix + "RoutineCompletions",
                           HabitsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.RoutineId)
                .IsRequired();

            builder.Property(x => x.LogDate)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            builder.Property(x => x.Completed)
                .IsRequired();

            builder.Property(x => x.TotalSteps);

            // Relationships
            builder.HasMany(x => x.CompletedSteps)
                .WithOne()
                .HasForeignKey("CompletionId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.RoutineId);
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => new { x.RoutineId, x.LogDate })
                .IsUnique();
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.TenantId);
        }
    }
}
