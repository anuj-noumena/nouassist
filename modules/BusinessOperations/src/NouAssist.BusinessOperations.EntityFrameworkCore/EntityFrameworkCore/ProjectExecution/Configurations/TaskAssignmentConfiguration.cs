using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.ProjectExecution.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.ProjectExecution.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for TaskAssignment entity
    /// </summary>
    public class TaskAssignmentConfiguration : IEntityTypeConfiguration<TaskAssignment>
    {
        public void Configure(EntityTypeBuilder<TaskAssignment> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();
            
            // Table mapping
            builder.ToTable(
                ProjectExecutionDbProperties.DbTablePrefix + "TaskAssignments",
                ProjectExecutionDbProperties.DbSchema);
                
            // Primary key - composite key of TaskId, UserId, and AssignedDate
            builder.HasKey(a => new { a.TaskId, a.UserId, a.AssignedDate });
            
            // Properties
            builder.Property(a => a.AssignedDate)
                .IsRequired();
                
            builder.Property(a => a.UnassignedDate);
                
            // Computed property IsActive - not mapped to a column
            builder.Ignore(a => a.IsActive);
            
            // Indexing
            builder.HasIndex(a => new { a.UserId, a.UnassignedDate });
        }
    }
}
