using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.ProjectExecution.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.ProjectExecution.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for ProjectMember entity
    /// </summary>
    public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
    {
        public void Configure(EntityTypeBuilder<ProjectMember> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();
            
            // Table mapping
            builder.ToTable(
                ProjectExecutionDbProperties.DbTablePrefix + "ProjectMembers",
                ProjectExecutionDbProperties.DbSchema);
                
            // Primary key - composite key of ProjectId and UserId
            builder.HasKey(m => new { m.ProjectId, m.UserId });
            
            // Properties
            builder.Property(m => m.Role)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(m => m.JoinDate)
                .IsRequired();
                
            builder.Property(m => m.LeaveDate);
                
            // Computed property IsActive - not mapped to a column
            builder.Ignore(m => m.IsActive);
            
            // Indexing
            builder.HasIndex(m => new { m.UserId, m.Role });
        }
    }
}
