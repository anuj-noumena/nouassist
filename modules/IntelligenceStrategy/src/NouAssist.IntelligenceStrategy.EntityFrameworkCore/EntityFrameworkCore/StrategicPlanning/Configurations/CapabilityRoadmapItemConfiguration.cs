using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity configuration for Capability Roadmap Item entity
    /// </summary>
    public class CapabilityRoadmapItemConfiguration : IEntityTypeConfiguration<CapabilityRoadmapItem>
    {
        public void Configure(EntityTypeBuilder<CapabilityRoadmapItem> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention(); 

            // Table Mapping
            builder.ToTable(StrategicPlanningDbProperties.DbTablePrefix + "CapabilityRoadmapItems", 
                            StrategicPlanningDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Foreign Key
            builder.Property(x => x.TransitionPlanId)
                .IsRequired();

            // Enum Mappings
            builder.Property(x => x.CurrentLevel)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.TargetLevel)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.TargetDate)
                .IsRequired();

            builder.Property(x => x.DevelopmentPath)
                .HasMaxLength(1000);

            builder.Property(x => x.InvestmentRequired)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            // Value Object Mapping - SkillName
            builder.OwnsOne(x => x.Skill, skillBuilder => 
            {
                skillBuilder.Property(s => s.Name)
                    .HasColumnName("SkillName")
                    .IsRequired()
                    .HasMaxLength(100);
                
                skillBuilder.Property(s => s.Category)
                    .HasColumnName("SkillCategory")
                    .IsRequired()
                    .HasMaxLength(50);

                skillBuilder.Property(s => s.Description)
                    .HasColumnName("SkillDescription")
                    .HasMaxLength(500);
            });

            // Collection Mapping - Required Actions as JSON
            builder.Property(x => x.RequiredActions)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null))
                .HasColumnType("jsonb");

            // Indexing
            builder.HasIndex(x => x.TransitionPlanId);
            builder.HasIndex(x => x.CurrentLevel);
            builder.HasIndex(x => x.TargetLevel);
            builder.HasIndex(x => x.TargetDate);
        }
    }
}
