using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations
{
    public class TaskOrganizationAggregateConfiguration : IEntityTypeConfiguration<TaskOrganizationAggregate>
    {
        public void Configure(EntityTypeBuilder<TaskOrganizationAggregate> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(WorkManagementDbProperties.DbTablePrefix + "TaskOrganizations",
                          WorkManagementDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Foreign Key to TaskInstance
            builder.Property(x => x.TaskInstanceId)
                .IsRequired();

            // GTD Organization Properties
            builder.Property(x => x.ProjectId);

            builder.Property(x => x.AreaId);

            builder.Property(x => x.ContextId);

            // Indexes
            builder.HasIndex(x => x.TaskInstanceId)
                .IsUnique(); // One-to-one relationship with TaskInstance

            builder.HasIndex(x => x.ProjectId);
            builder.HasIndex(x => x.AreaId);
            builder.HasIndex(x => x.ContextId);

            // Concurrency
            builder.Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken()
                .HasMaxLength(40);
        }
    }
}
