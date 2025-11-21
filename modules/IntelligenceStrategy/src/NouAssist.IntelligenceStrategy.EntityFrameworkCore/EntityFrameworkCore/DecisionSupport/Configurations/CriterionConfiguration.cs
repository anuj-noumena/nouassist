using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.DecisionSupport.Constants;
using NouAssist.IntelligenceStrategy.DecisionSupport.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for the <see cref="Criterion"/> entity
    /// </summary>
    public class CriterionConfiguration : IEntityTypeConfiguration<Criterion>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Criterion> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();
            
            // Table Mapping
            builder.ToTable(
                DecisionSupportDbProperties.DbTablePrefix + "Criteria", 
                DecisionSupportDbProperties.DbSchema);
                
            // Primary Key
            builder.HasKey(x => x.Id);
            
            // Property Mappings
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(DecisionSupportConsts.MaxDescriptionLength);
                
            builder.Property(x => x.Weight)
                .IsRequired()
                .HasPrecision(5, 2); // Supports values with up to 5 digits, 2 of which are decimals
                
            // Indexing
            builder.HasIndex(x => x.DecisionModelId);
        }
    }
}
