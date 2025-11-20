using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.PersonalDevelopment.Habits;
using NouAssist.PersonalDevelopment.Habits.Entities;

namespace NouAssist.PersonalDevelopment.EntityFrameworkCore.Habits.Configurations
{
    public class RoutineStepConfiguration : IEntityTypeConfiguration<RoutineStep>
    {
        public void Configure(EntityTypeBuilder<RoutineStep> builder)
        {
            builder.ToTable(PersonalDevelopmentDbProperties.DbTablePrefix + "RoutineSteps",
                           PersonalDevelopmentDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.RoutineId)
                .IsRequired();

            builder.Property(x => x.StepNumber)
                .IsRequired();

            builder.Property(x => x.HabitId);

            builder.Property(x => x.AdhocTaskDescription)
                .HasMaxLength(500);

            builder.Property(x => x.EstimatedDurationMinutes);

            builder.HasIndex(x => x.RoutineId);
            builder.HasIndex(x => new { x.RoutineId, x.StepNumber });
        }
    }
}
