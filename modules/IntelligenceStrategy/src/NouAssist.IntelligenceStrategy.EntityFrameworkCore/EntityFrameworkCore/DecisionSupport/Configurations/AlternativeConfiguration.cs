using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.DecisionSupport.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for the <see cref="Alternative"/> entity
    /// </summary>
    public class AlternativeConfiguration : IEntityTypeConfiguration<Alternative>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Alternative> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();
            
            // Table Mapping
            builder.ToTable(
                DecisionSupportDbProperties.DbTablePrefix + "Alternatives", 
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
                
            // Relationship Mapping (One-to-Many for scores)
            builder.HasMany(x => x.Scores)
                .WithOne()
                .HasForeignKey(s => s.AlternativeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
                
            // Indexing
            builder.HasIndex(x => x.DecisionModelId);
        }
    }
}
