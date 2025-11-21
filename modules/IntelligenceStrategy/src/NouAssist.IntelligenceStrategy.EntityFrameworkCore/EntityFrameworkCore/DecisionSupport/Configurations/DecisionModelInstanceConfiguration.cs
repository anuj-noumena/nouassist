using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using NouAssist.IntelligenceStrategy.DecisionSupport.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for the <see cref="DecisionModelInstance"/> aggregate
    /// </summary>
    public class DecisionModelInstanceConfiguration : IEntityTypeConfiguration<DecisionModelInstance>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<DecisionModelInstance> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention();
            
            // Table Mapping
            builder.ToTable(
                DecisionSupportDbProperties.DbTablePrefix + "DecisionModelInstances", 
                DecisionSupportDbProperties.DbSchema);

            // Primary Key (handled by Entity<TKey> / AggregateRoot<TKey>)
            builder.HasKey(x => x.Id);
            
            // Property Mappings
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(DecisionSupportConsts.MaxTitleLength);
                
            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(DecisionSupportConsts.MaxDescriptionLength);
                
            builder.Property(x => x.ModelType)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(x => x.IsFinalized)
                .IsRequired();
                
            builder.Property(x => x.DecisionDate)
                .IsRequired(false);
                
            builder.Property(x => x.SelectedAlternativeId)
                .IsRequired(false);
                
            // Relationship Mapping (One-to-Many for criteria)
            builder.HasMany(x => x.Criteria)
                .WithOne()
                .HasForeignKey(c => c.DecisionModelId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
                
            // Relationship Mapping (One-to-Many for alternatives)
            builder.HasMany(x => x.Alternatives)
                .WithOne()
                .HasForeignKey(a => a.DecisionModelId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
                
            // Indexing
            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.IsFinalized);
        }
    }
}
