using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.DecisionSupport.Constants;
using NouAssist.IntelligenceStrategy.DecisionSupport.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for the <see cref="AnalysisReference"/> entity
    /// </summary>
    public class AnalysisReferenceConfiguration : IEntityTypeConfiguration<AnalysisReference>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<AnalysisReference> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();
            
            // Table Mapping
            builder.ToTable(
                DecisionSupportDbProperties.DbTablePrefix + "AnalysisReferences", 
                DecisionSupportDbProperties.DbSchema);
                
            // Composite Primary Key
            builder.HasKey(x => new { x.StrategicInitiativeId, x.ReferenceType, x.ReferenceKey });
            
            // Property Mappings
            builder.Property(x => x.ReferenceType)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(x => x.ReferenceKey)
                .IsRequired()
                .HasMaxLength(DecisionSupportConsts.MaxReferenceKeyLength);
                
            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(DecisionSupportConsts.MaxDescriptionLength);
                
            // Indexing
            builder.HasIndex(x => x.StrategicInitiativeId);
        }
    }
}
