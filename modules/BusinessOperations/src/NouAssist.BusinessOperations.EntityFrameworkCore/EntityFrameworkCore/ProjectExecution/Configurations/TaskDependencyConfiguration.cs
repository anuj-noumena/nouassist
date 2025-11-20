using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.ProjectExecution.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.ProjectExecution.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for TaskDependency entity
    /// </summary>
    public class TaskDependencyConfiguration : IEntityTypeConfiguration<TaskDependency>
    {
        public void Configure(EntityTypeBuilder<TaskDependency> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();
            
            // Table mapping
            builder.ToTable(
                ProjectExecutionDbProperties.DbTablePrefix + "TaskDependencies",
                ProjectExecutionDbProperties.DbSchema);
                
            // Primary key - composite key of TaskId, DependsOnTaskId, and DependencyType
            builder.HasKey(d => new { d.TaskId, d.DependsOnTaskId, d.DependencyType });
            
            // Properties
            builder.Property(d => d.DependencyType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);
                
            // Foreign key relationship to the dependent Task
            builder.HasOne<Task>()
                .WithMany()
                .HasForeignKey(d => d.DependsOnTaskId)
                .OnDelete(DeleteBehavior.NoAction); // Use NoAction to prevent circular cascade delete
        }
    }
}
