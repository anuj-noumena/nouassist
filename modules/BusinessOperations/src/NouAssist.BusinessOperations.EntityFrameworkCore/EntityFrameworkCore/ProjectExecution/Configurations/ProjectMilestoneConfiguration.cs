using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.ProjectExecution.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.ProjectExecution.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for ProjectMilestone entity
    /// </summary>
    public class ProjectMilestoneConfiguration : IEntityTypeConfiguration<ProjectMilestone>
    {
        public void Configure(EntityTypeBuilder<ProjectMilestone> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();
            
            // Table mapping
            builder.ToTable(
                ProjectExecutionDbProperties.DbTablePrefix + "ProjectMilestones",
                ProjectExecutionDbProperties.DbSchema);
                
            // Primary key - composite key of ProjectId and Name
            builder.HasKey(m => new { m.ProjectId, m.Name });
            
            // Properties
            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(ProjectExecutionConsts.MaxProjectMilestoneNameLength);
                
            builder.Property(m => m.TargetDate)
                .IsRequired();
                
            builder.Property(m => m.Description)
                .HasMaxLength(1000);
                
            builder.Property(m => m.CompletedDate);
                
            // Computed property IsCompleted - not mapped to a column
            builder.Ignore(m => m.IsCompleted);
            
            // Indexing
            builder.HasIndex(m => new { m.ProjectId, m.TargetDate });
        }
    }
}
