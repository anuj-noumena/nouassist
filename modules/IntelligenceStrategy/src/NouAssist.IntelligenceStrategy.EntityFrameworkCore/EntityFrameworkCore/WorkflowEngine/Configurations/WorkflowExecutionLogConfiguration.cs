using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.WorkflowEngine.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Configuration for the WorkflowExecutionLog entity
    /// </summary>
    public class WorkflowExecutionLogConfiguration : IEntityTypeConfiguration<WorkflowExecutionLog>
    {
        /// <summary>
        /// Configures the entity type
        /// </summary>
        /// <param name="builder">The entity type builder</param>
        public void Configure(EntityTypeBuilder<WorkflowExecutionLog> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(
                WorkflowEngineDbProperties.DbTablePrefix + "ExecutionLogs", 
                WorkflowEngineDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.LogType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(1000);

            // Data Dictionary
            builder.Property(x => x.Data)
                .HasColumnName("Data")
                .HasJsonConversion();

            // Indexing
            builder.HasIndex(x => x.WorkflowInstanceId);
            builder.HasIndex(x => x.ActivityInstanceId);
            builder.HasIndex(x => x.Timestamp);
            builder.HasIndex(x => x.LogType);
        }
    }
}
