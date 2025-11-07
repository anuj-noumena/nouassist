using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.CoreProductivity.HabitSystems.Entities;

namespace NouAssist.CoreProductivity.HabitSystems.EntityFrameworkCore.Configurations
{
    public class RoutineCompletedStepConfiguration : IEntityTypeConfiguration<RoutineCompletedStep>
    {
        public void Configure(EntityTypeBuilder<RoutineCompletedStep> builder)
        {
            builder.ToTable(HabitsDbProperties.DbTablePrefix + "RoutineCompletedSteps",
                           HabitsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CompletionId)
                .IsRequired();

            builder.Property(x => x.StepId)
                .IsRequired();

            builder.Property(x => x.CompletedAt)
                .IsRequired();

            builder.HasIndex(x => x.CompletionId);
            builder.HasIndex(x => x.StepId);
        }
    }
}
