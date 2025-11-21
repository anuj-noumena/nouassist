using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using NouAssist.BusinessOperations.ProjectExecution.Constants;
using NouAssist.BusinessOperations.ProjectExecution.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.ProjectExecution.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for Project aggregate
    /// </summary>
    public class ProjectAggregateConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention();
            
            // Table mapping
            builder.ToTable(
                ProjectExecutionDbProperties.DbTablePrefix + "Projects",
                ProjectExecutionDbProperties.DbSchema);
                
            // Primary key is configured by convention
            
            // Value object mapping with OwnsOne - ProjectDetails
            builder.OwnsOne(p => p.Details, detailsBuilder =>
            {
                detailsBuilder.Property(d => d.Name)
                    .HasColumnName("Name")
                    .IsRequired()
                    .HasMaxLength(ProjectExecutionConsts.MaxProjectNameLength);
                    
                detailsBuilder.Property(d => d.Description)
                    .HasColumnName("Description")
                    .IsRequired()
                    .HasMaxLength(ProjectExecutionConsts.MaxProjectDescriptionLength);
                    
                detailsBuilder.Property(d => d.StartDate)
                    .HasColumnName("StartDate")
                    .IsRequired();
                    
                detailsBuilder.Property(d => d.PlannedEndDate)
                    .HasColumnName("PlannedEndDate")
                    .IsRequired();
            });
            
            // Simple properties
            builder.Property(p => p.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);
                
            builder.Property(p => p.ActualEndDate);
            
            builder.Property(p => p.OrganizationId);
            
            builder.Property(p => p.ContactId);
            
            // Relationships - one-to-many
            builder.HasMany(p => p.Phases)
                .WithOne()
                .HasForeignKey(ph => ph.ProjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(p => p.Milestones)
                .WithOne()
                .HasForeignKey(m => m.ProjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(p => p.Members)
                .WithOne()
                .HasForeignKey(m => m.ProjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
                
            // Indexing
            builder.HasIndex(p => new { p.TenantId, p.Status });
            builder.HasIndex(p => new { p.TenantId, p.OrganizationId });
            builder.HasIndex(p => new { p.TenantId, p.ContactId });
        }
    }
}
