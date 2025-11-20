using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for Opportunity Management Record aggregate
    /// </summary>
    public class OpportunityManagementRecordConfiguration : IEntityTypeConfiguration<OpportunityManagementRecord>
    {
        public void Configure(EntityTypeBuilder<OpportunityManagementRecord> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention(); 

            // Table Mapping
            builder.ToTable(StrategicPlanningDbProperties.DbTablePrefix + "OpportunityManagementRecords", 
                            StrategicPlanningDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.RecordName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);

            // Enum Mapping
            builder.Property(x => x.CurrentFocusLevel)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Relationship Mapping - One-to-Many for OpportunityFilterRules
            builder.HasMany<OpportunityFilterRule>()
                   .WithOne()
                   .HasForeignKey(f => f.OpportunityManagementRecordId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Collection Mapping - Strategic Experiments as JSON
            builder.Property(x => x.StrategicExperiments)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnType("jsonb");

            // Indexing
            builder.HasIndex(x => x.CurrentFocusLevel);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.CreatedDate);
            builder.HasIndex(x => x.TenantId);

            // Extra Properties
            builder.Property(x => x.ExtraProperties)
                .HasColumnName(nameof(OpportunityManagementRecord.ExtraProperties))
                .HasColumnType("jsonb");

            // Concurrency Stamp
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
