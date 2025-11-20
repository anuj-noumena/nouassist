using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.DecisionSupport.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for the <see cref="AlternativeScore"/> entity
    /// </summary>
    public class AlternativeScoreConfiguration : IEntityTypeConfiguration<AlternativeScore>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<AlternativeScore> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();
            
            // Table Mapping
            builder.ToTable(
                DecisionSupportDbProperties.DbTablePrefix + "AlternativeScores", 
                DecisionSupportDbProperties.DbSchema);
                
            // Composite Primary Key
            builder.HasKey(x => new { x.AlternativeId, x.CriterionId });
            
            // Property Mappings
            builder.Property(x => x.Score)
                .IsRequired()
                .HasPrecision(5, 2); // Supports values with up to 5 digits, 2 of which are decimals
                
            // Indexing
            builder.HasIndex(x => x.AlternativeId);
            builder.HasIndex(x => x.CriterionId);
        }
    }
}
