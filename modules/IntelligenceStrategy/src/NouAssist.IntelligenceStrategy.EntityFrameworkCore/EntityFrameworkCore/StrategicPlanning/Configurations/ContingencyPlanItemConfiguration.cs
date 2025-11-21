using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Constants;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for Contingency Plan Item entity
    /// </summary>
    public class ContingencyPlanItemConfiguration : IEntityTypeConfiguration<ContingencyPlanItem>
    {
        public void Configure(EntityTypeBuilder<ContingencyPlanItem> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention(); 

            // Table Mapping
            builder.ToTable(StrategicPlanningDbProperties.DbTablePrefix + "ContingencyPlanItems", 
                            StrategicPlanningDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Foreign Key
            builder.Property(x => x.CrisisManagementPlanId)
                .IsRequired();

            // Property Mappings
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(StrategicPlanningConsts.MaxGoalTitleLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(StrategicPlanningConsts.MaxContingencyDescriptionLength);

            builder.Property(x => x.Priority)
                .IsRequired();

            builder.Property(x => x.ResponsibleUserId)
                .IsRequired(false);

            // Value Object Mapping - RiskScenario
            builder.OwnsOne(x => x.RiskScenario, riskBuilder => 
            {
                riskBuilder.Property(r => r.Description)
                    .HasColumnName("RiskDescription")
                    .IsRequired()
                    .HasMaxLength(StrategicPlanningConsts.MaxContingencyDescriptionLength);
                
                riskBuilder.Property(r => r.Level)
                    .HasColumnName("RiskLevel")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                riskBuilder.Property(r => r.ProbabilityScore)
                    .HasColumnName("ProbabilityScore")
                    .IsRequired()
                    .HasColumnType("decimal(5,4)");

                riskBuilder.Property(r => r.ImpactScore)
                    .HasColumnName("ImpactScore")
                    .IsRequired()
                    .HasColumnType("decimal(5,4)");
            });

            // Collection Mapping - Action Steps as JSON
            builder.Property(x => x.ActionSteps)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnType("jsonb");

            // Collection Mapping - Required Resources as JSON
            builder.Property(x => x.RequiredResources)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnType("jsonb");

            // Indexing
            builder.HasIndex(x => x.CrisisManagementPlanId);
            builder.HasIndex(x => x.Priority);
            builder.HasIndex(x => x.ResponsibleUserId);
        }
    }
}
