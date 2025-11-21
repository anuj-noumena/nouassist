using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using NouAssist.IntelligenceStrategy.DecisionSupport.Constants;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;
using NouAssist.IntelligenceStrategy.DecisionSupport.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// EF Core configuration for the <see cref="StrategicInitiativeProposal"/> aggregate
    /// </summary>
    public class StrategicInitiativeProposalConfiguration : IEntityTypeConfiguration<StrategicInitiativeProposal>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<StrategicInitiativeProposal> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention();
            
            // Table Mapping
            builder.ToTable(
                DecisionSupportDbProperties.DbTablePrefix + "StrategicInitiativeProposals", 
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
                
            builder.Property(x => x.RejectionReason)
                .HasMaxLength(DecisionSupportConsts.MaxDescriptionLength);
                
            // Value Object - Rationale
            builder.OwnsOne(x => x.Rationale, rationaleBuilder =>
            {
                rationaleBuilder.Property(r => r.Value)
                    .HasColumnName("Rationale")
                    .IsRequired()
                    .HasMaxLength(DecisionSupportConsts.MaxRationaleLength);
            });
            
            // Value Object - ExpectedOutcomes
            builder.OwnsOne(x => x.ExpectedOutcomes, outcomesBuilder =>
            {
                outcomesBuilder.Property(o => o.Value)
                    .HasColumnName("ExpectedOutcomes")
                    .IsRequired()
                    .HasMaxLength(DecisionSupportConsts.MaxExpectedOutcomesLength);
            });
            
            // Value Object - ProjectId
            builder.OwnsOne(x => x.ProjectId, projectIdBuilder =>
            {
                projectIdBuilder.Property(p => p.Value)
                    .HasColumnName("ProjectId");
            });
            
            // Enum Mapping - ProposalStatus
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
                
            // Enum Mapping - AlignmentStatus
            builder.Property(x => x.StrategicAlignment)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
                
            // Date properties
            builder.Property(x => x.ProposedDate)
                .IsRequired();
                
            builder.Property(x => x.DecisionDate)
                .IsRequired(false);
                
            // Foreign Key to DecisionModelInstance
            builder.Property(x => x.RelatedDecisionModelId)
                .IsRequired(false);
                
            // Relationship Mapping (One-to-Many for supporting analyses)
            builder.HasMany(x => x.SupportingAnalyses)
                .WithOne()
                .HasForeignKey(a => a.StrategicInitiativeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
                
            // Indexing
            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.RelatedDecisionModelId);
        }
    }
}
