using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using NouAssist.BusinessOperations.ProjectExecution.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.BusinessOperations.ProjectExecution.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for TimeLog aggregate
    /// </summary>
    public class TimeLogAggregateConfiguration : IEntityTypeConfiguration<TimeLog>
    {
        public void Configure(EntityTypeBuilder<TimeLog> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();
            
            // Table mapping
            builder.ToTable(
                ProjectExecutionDbProperties.DbTablePrefix + "TimeLogs",
                ProjectExecutionDbProperties.DbSchema);
                
            // Primary key is configured by convention
            
            // Value object mapping with OwnsOne - TimeLogDetails
            builder.OwnsOne(tl => tl.Details, detailsBuilder =>
            {
                detailsBuilder.Property(d => d.Hours)
                    .HasColumnName("Hours")
                    .IsRequired();
                    
                detailsBuilder.Property(d => d.LogDate)
                    .HasColumnName("LogDate")
                    .IsRequired();
                    
                detailsBuilder.Property(d => d.Notes)
                    .HasColumnName("Notes")
                    .HasMaxLength(ProjectExecutionConsts.MaxTimeLogNotesLength);
                    
                detailsBuilder.Property(d => d.ResourceInstanceId)
                    .HasColumnName("ResourceInstanceId");
            });
            
            // Simple properties
            builder.Property(tl => tl.UserId)
                .IsRequired();
                
            builder.Property(tl => tl.ProjectId)
                .IsRequired();
                
            builder.Property(tl => tl.TaskId);
            
            // Foreign key relationships
            builder.HasOne<Project>()
                .WithMany()
                .HasForeignKey(tl => tl.ProjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasOne<Task>()
                .WithMany()
                .HasForeignKey(tl => tl.TaskId)
                .OnDelete(DeleteBehavior.SetNull);
                
            // Indexing
            builder.HasIndex(tl => new { tl.TenantId, tl.UserId, tl.ProjectId });
            builder.HasIndex(tl => new { tl.TenantId, tl.TaskId });
            builder.HasIndex(tl => new { tl.TenantId, tl.Details.LogDate });
        }
    }
}
