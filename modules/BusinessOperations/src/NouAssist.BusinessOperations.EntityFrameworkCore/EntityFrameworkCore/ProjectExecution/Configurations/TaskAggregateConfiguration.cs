using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.ProjectExecution.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for Task aggregate
    /// </summary>
    public class TaskAggregateConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();
            
            // Table mapping
            builder.ToTable(
                ProjectExecutionDbProperties.DbTablePrefix + "Tasks",
                ProjectExecutionDbProperties.DbSchema);
                
            // Primary key is configured by convention
            
            // Value object mapping with OwnsOne - TaskDetails
            builder.OwnsOne(t => t.Details, detailsBuilder =>
            {
                detailsBuilder.Property(d => d.Name)
                    .HasColumnName("Name")
                    .IsRequired()
                    .HasMaxLength(ProjectExecutionConsts.MaxTaskNameLength);
                    
                detailsBuilder.Property(d => d.Description)
                    .HasColumnName("Description")
                    .IsRequired()
                    .HasMaxLength(ProjectExecutionConsts.MaxTaskDescriptionLength);
                    
                detailsBuilder.Property(d => d.EstimatedHours)
                    .HasColumnName("EstimatedHours")
                    .IsRequired();
                    
                detailsBuilder.Property(d => d.Priority)
                    .HasColumnName("Priority")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);
            });
            
            // Simple properties
            builder.Property(t => t.ProjectId)
                .IsRequired();
                
            builder.Property(t => t.PhaseId);
            
            builder.Property(t => t.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);
                
            builder.Property(t => t.StartDate);
            
            builder.Property(t => t.DueDate);
            
            builder.Property(t => t.CompletionDate);
            
            // Relationships - one-to-many
            builder.HasMany(t => t.Assignments)
                .WithOne()
                .HasForeignKey(a => a.TaskId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(t => t.Dependencies)
                .WithOne()
                .HasForeignKey(d => d.TaskId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
                
            // Foreign key relationship to Project
            builder.HasOne<Project>()
                .WithMany()
                .HasForeignKey(t => t.ProjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
                
            // Indexing
            builder.HasIndex(t => new { t.TenantId, t.ProjectId, t.Status });
            builder.HasIndex(t => new { t.TenantId, t.PhaseId });
            builder.HasIndex(t => new { t.TenantId, t.DueDate });
        }
    }
}
