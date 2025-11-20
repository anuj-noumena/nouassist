using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Entities;
using NouAssist.RequirementsManagement.AnalysisPrioritization.ValueObjects;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.EntityFrameworkCore.Configurations
{
    public class StakeholderVoteConfiguration : IEntityTypeConfiguration<StakeholderVote>
    {
        public void Configure(EntityTypeBuilder<StakeholderVote> builder)
        {
            builder.ToTable(RequirementsAnalysisToolsDbProperties.DbTablePrefix + "StakeholderVotes",
                           RequirementsAnalysisToolsDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.PrioritizationActivityId)
                .IsRequired()
                .HasColumnName(nameof(StakeholderVote.PrioritizationActivityId));

            builder.Property(x => x.RequirementId)
                .IsRequired()
                .HasColumnName(nameof(StakeholderVote.RequirementId));

            builder.Property(x => x.StakeholderId)
                .IsRequired()
                .HasColumnName(nameof(StakeholderVote.StakeholderId));

            builder.Property(x => x.Comment)
                .HasMaxLength(1000)
                .HasColumnName(nameof(StakeholderVote.Comment));

            builder.Property(x => x.VotedAt)
                .IsRequired()
                .HasColumnName(nameof(StakeholderVote.VotedAt));

            // Value Object: VoteValue
            builder.OwnsOne(x => x.Vote, voteBuilder =>
            {
                voteBuilder.Property(v => v.NumericValue)
                    .HasColumnName("VoteNumericValue")
                    .HasColumnType("decimal(10,4)")
                    .IsRequired();

                voteBuilder.Property(v => v.StringValue)
                    .HasColumnName("VoteStringValue")
                    .HasMaxLength(100);

                voteBuilder.Property(v => v.Method)
                    .HasColumnName("VoteMethod")
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();
            });

            // Indexes
            builder.HasIndex(x => new { x.PrioritizationActivityId, x.StakeholderId, x.RequirementId })
                .IsUnique()
                .HasDatabaseName("IX_StakeholderVotes_Activity_Stakeholder_Requirement");

            builder.HasIndex(x => x.PrioritizationActivityId)
                .HasDatabaseName("IX_StakeholderVotes_PrioritizationActivityId");

            builder.HasIndex(x => x.RequirementId)
                .HasDatabaseName("IX_StakeholderVotes_RequirementId");

            builder.HasIndex(x => x.StakeholderId)
                .HasDatabaseName("IX_StakeholderVotes_StakeholderId");

            builder.HasIndex(x => x.VotedAt)
                .HasDatabaseName("IX_StakeholderVotes_VotedAt");
        }
    }
}
