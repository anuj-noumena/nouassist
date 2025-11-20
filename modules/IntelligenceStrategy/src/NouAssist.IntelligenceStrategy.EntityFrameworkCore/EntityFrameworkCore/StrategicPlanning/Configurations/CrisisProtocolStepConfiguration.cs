using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for Crisis Protocol Step entity
    /// </summary>
    public class CrisisProtocolStepConfiguration : IEntityTypeConfiguration<CrisisProtocolStep>
    {
        public void Configure(EntityTypeBuilder<CrisisProtocolStep> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention(); 

            // Table Mapping
            builder.ToTable(StrategicPlanningDbProperties.DbTablePrefix + "CrisisProtocolSteps", 
                            StrategicPlanningDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Foreign Key
            builder.Property(x => x.CrisisManagementPlanId)
                .IsRequired();

            // Property Mappings
            builder.Property(x => x.StepName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.OrderSequence)
                .IsRequired();

            builder.Property(x => x.MaxExecutionTime)
                .IsRequired();

            builder.Property(x => x.IsAutomated)
                .IsRequired()
                .HasDefaultValue(false);

            // Value Object Mapping - EmergencyProcedure
            builder.OwnsOne(x => x.Procedure, procBuilder => 
            {
                procBuilder.Property(p => p.Name)
                    .HasColumnName("ProcedureName")
                    .IsRequired()
                    .HasMaxLength(100);
                
                procBuilder.Property(p => p.Description)
                    .HasColumnName("ProcedureDescription")
                    .IsRequired()
                    .HasMaxLength(1000);

                procBuilder.Property(p => p.RequiredLevel)
                    .HasColumnName("RequiredCrisisLevel")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                procBuilder.Property(p => p.EstimatedDuration)
                    .HasColumnName("EstimatedDuration")
                    .IsRequired();

                procBuilder.Property(p => p.Steps)
                    .HasColumnName("ProcedureSteps")
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null))
                    .HasColumnType("jsonb");
            });

            // Collection Mapping - Required Roles as JSON
            builder.Property(x => x.RequiredRoles)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnType("jsonb");

            // Indexing
            builder.HasIndex(x => x.CrisisManagementPlanId);
            builder.HasIndex(x => x.OrderSequence);
            builder.HasIndex(x => x.IsAutomated);
        }
    }
}
