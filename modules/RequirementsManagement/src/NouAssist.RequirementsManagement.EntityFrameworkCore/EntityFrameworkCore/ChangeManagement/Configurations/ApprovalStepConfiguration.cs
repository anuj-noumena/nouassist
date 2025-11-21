using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.ChangeManagement.Constants;
using NouAssist.RequirementsManagement.ChangeManagement.Entities;
using NouAssist.RequirementsManagement.ChangeManagement.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.ChangeManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity type configuration for ApprovalStep entity
    /// </summary>
    public class ApprovalStepConfiguration : IEntityTypeConfiguration<ApprovalStep>
    {
        public void Configure(EntityTypeBuilder<ApprovalStep> builder)
        {
            // Apply ABP base conventions for audited entity
            builder.ConfigureByConvention();

            // Table mapping
            builder.ToTable(
                ChangeManagementDbProperties.DbTablePrefix + "ApprovalSteps",
                ChangeManagementDbProperties.DbSchema);

            // Primary key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.ChangeRequestId)
                .IsRequired();

            builder.Property(x => x.ApproverId)
                .IsRequired();

            builder.Property(x => x.SequenceOrder)
                .IsRequired();

            builder.Property(x => x.Decision)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.DecisionDate);

            // Value object mappings
            builder.OwnsOne(x => x.ApproverRole, roleBuilder =>
            {
                roleBuilder.Property(r => r.Name)
                    .HasColumnName("ApproverRoleName")
                    .IsRequired()
                    .HasMaxLength(ChangeManagementConsts.MaxApproverRoleLength);

                roleBuilder.Property(r => r.Priority)
                    .HasColumnName("ApproverRolePriority");

                roleBuilder.WithOwner();
            });

            builder.OwnsOne(x => x.DecisionReason, reasonBuilder =>
            {
                reasonBuilder.Property(r => r.Value)
                    .HasColumnName("DecisionReason")
                    .HasMaxLength(ChangeManagementConsts.MaxDecisionReasonLength);

                reasonBuilder.WithOwner();
            });

            // Indexes
            builder.HasIndex(x => x.ChangeRequestId);
            builder.HasIndex(x => x.ApproverId);
            builder.HasIndex(x => x.Decision);
            builder.HasIndex(x => new { x.ChangeRequestId, x.SequenceOrder }).IsUnique();
        }
    }
}
