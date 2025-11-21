using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.ProjectExecution.Constants;
using NouAssist.BusinessOperations.ProjectExecution.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.ProjectExecution.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for ProjectPhase entity
    /// </summary>
    public class ProjectPhaseConfiguration : IEntityTypeConfiguration<ProjectPhase>
    {
        public void Configure(EntityTypeBuilder<ProjectPhase> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();
            
            // Table mapping
            builder.ToTable(
                ProjectExecutionDbProperties.DbTablePrefix + "ProjectPhases",
                ProjectExecutionDbProperties.DbSchema);
                
            // Primary key - composite key of ProjectId and Name
            builder.HasKey(ph => new { ph.ProjectId, ph.Name });
            
            // Properties
            builder.Property(ph => ph.Name)
                .IsRequired()
                .HasMaxLength(ProjectExecutionConsts.MaxProjectPhaseNameLength);
                
            builder.Property(ph => ph.StartDate)
                .IsRequired();
                
            builder.Property(ph => ph.EndDate)
                .IsRequired();
                
            builder.Property(ph => ph.Order)
                .IsRequired();
                
            // Indexing
            builder.HasIndex(ph => new { ph.ProjectId, ph.Order });
        }
    }
}
