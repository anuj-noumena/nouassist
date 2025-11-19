using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Aggregates.Task;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class TaskAggregateConfiguration : IEntityTypeConfiguration<TaskAggregate>
    {
        public void Configure(EntityTypeBuilder<TaskAggregate> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkManagementDbProperties.DbTablePrefix + "Tasks",
                          WorkManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Core Properties
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(TasksDbConsts.MaxTitleLength);

            builder.Property(x => x.Description)
                .HasMaxLength(TasksDbConsts.MaxDescriptionLength);

            builder.Property(x => x.ExpectedResult)
                .HasMaxLength(TasksDbConsts.MaxExpectedResultLength);

            builder.Property(x => x.Notes)
                .HasMaxLength(TasksDbConsts.MaxNotesLength);

            builder.Property(x => x.OwnerId)
                .IsRequired();

            // GTD Organization
            builder.Property(x => x.AreaId);

            builder.Property(x => x.ContextId);

            builder.Property(x => x.ProjectId);

            builder.Property(x => x.TaskTypeId);

            // State
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.Version)
                .IsRequired()
                .IsConcurrencyToken();

            // References to other aggregates
            builder.Property(x => x.DefinitionId);
            builder.Property(x => x.DefaultsId);
            builder.Property(x => x.RecurrenceId);
            builder.Property(x => x.HierarchyId);

            // Indexes
            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => x.AreaId);
            builder.HasIndex(x => x.ContextId);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => new { x.TenantId, x.IsDeleted });
            builder.HasIndex(x => new { x.AreaId, x.IsActive });
            builder.HasIndex(x => new { x.ProjectId, x.IsActive });
            builder.HasIndex(x => x.DefinitionId);
            builder.HasIndex(x => x.DefaultsId);
            builder.HasIndex(x => x.RecurrenceId);
            builder.HasIndex(x => x.HierarchyId);

            // Multi-tenancy
            builder.HasIndex(x => x.TenantId);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);
        }
    }
}