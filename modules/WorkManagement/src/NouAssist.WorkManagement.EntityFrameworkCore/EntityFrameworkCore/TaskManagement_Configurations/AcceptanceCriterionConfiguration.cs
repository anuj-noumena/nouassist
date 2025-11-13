using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class AcceptanceCriterionConfiguration : IEntityTypeConfiguration<AcceptanceCriterion>
    {
        public void Configure(EntityTypeBuilder<AcceptanceCriterion> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(TasksDbProperties.DbTablePrefix + "AcceptanceCriteria",
                          TasksDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.TaskId)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(TasksDbConsts.MaxCriterionDescriptionLength);

            builder.Property(x => x.GivenContext)
                .HasMaxLength(TasksDbConsts.MaxGivenContextLength);

            builder.Property(x => x.WhenAction)
                .HasMaxLength(TasksDbConsts.MaxWhenActionLength);

            builder.Property(x => x.ThenOutcome)
                .HasMaxLength(TasksDbConsts.MaxThenOutcomeLength);

            builder.Property(x => x.TestScenario)
                .HasMaxLength(TasksDbConsts.MaxTestScenarioLength);

            builder.Property(x => x.Notes)
                .HasMaxLength(TasksDbConsts.MaxCriterionNotesLength);

            builder.Property(x => x.Priority);

            // Indexes
            builder.HasIndex(x => x.TaskId);
            builder.HasIndex(x => new { x.TaskId, x.Priority });

            // Concurrency
            // builder.Property(x => x.ConcurrencyStamp)
            //     .IsConcurrencyToken()
            //     .HasMaxLength(40);
        }
    }
}
