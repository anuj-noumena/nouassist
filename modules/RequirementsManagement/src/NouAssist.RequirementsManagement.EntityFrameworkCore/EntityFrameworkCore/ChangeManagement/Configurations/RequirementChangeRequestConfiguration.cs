using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.ChangeManagement.Aggregates;
using NouAssist.RequirementsManagement.ChangeManagement.Constants;
using NouAssist.RequirementsManagement.ChangeManagement.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.ChangeManagement.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// Entity type configuration for RequirementChangeRequest aggregate
    /// </summary>
    public class RequirementChangeRequestConfiguration : IEntityTypeConfiguration<RequirementChangeRequest>
    {
        public void Configure(EntityTypeBuilder<RequirementChangeRequest> builder)
        {
            // Apply ABP base conventions (audit properties, multi-tenancy, etc.)
            builder.ConfigureByConvention();

            // Table mapping with prefix
            builder.ToTable(
                ChangeManagementDbProperties.DbTablePrefix + "ChangeRequests", 
                ChangeManagementDbProperties.DbSchema);

            // Primary key (already handled by ConfigureByConvention but explicitly defined for clarity)
            builder.HasKey(x => x.Id);

            // Value object mappings
            builder.OwnsOne(x => x.RequestIdentifier, requestIdBuilder =>
            {
                requestIdBuilder.Property(r => r.Value)
                    .HasColumnName("RequestIdentifier")
                    .IsRequired()
                    .HasMaxLength(ChangeManagementConsts.MaxRequestIdentifierLength);

                requestIdBuilder.WithOwner();
            });

            builder.OwnsOne(x => x.AffectedRequirement, refBuilder =>
            {
                refBuilder.Property(r => r.RequirementId)
                    .HasColumnName("AffectedRequirementId")
                    .IsRequired();

                refBuilder.Property(r => r.Version)
                    .HasColumnName("AffectedRequirementVersion")
                    .IsRequired()
                    .HasMaxLength(50);

                refBuilder.WithOwner();
            });

            // Basic properties
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(ChangeManagementConsts.MaxTitleLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(ChangeManagementConsts.MaxDescriptionLength);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.SubmitterId)
                .IsRequired();

            builder.Property(x => x.SubmissionDate)
                .IsRequired();

            builder.Property(x => x.CompletionDate);

            // Relationships (assuming entities are configured in separate configuration classes)
            builder.HasMany(x => x.Rationales)
                .WithOne()
                .HasForeignKey(r => r.ChangeRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.ImpactStatements)
                .WithOne()
                .HasForeignKey(i => i.ChangeRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.ApprovalSteps)
                .WithOne()
                .HasForeignKey(a => a.ChangeRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex("RequestIdentifier_Value").IsUnique();
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.SubmissionDate);
            builder.HasIndex(x => x.SubmitterId);
            builder.HasIndex("AffectedRequirementId");
        }
    }
}
