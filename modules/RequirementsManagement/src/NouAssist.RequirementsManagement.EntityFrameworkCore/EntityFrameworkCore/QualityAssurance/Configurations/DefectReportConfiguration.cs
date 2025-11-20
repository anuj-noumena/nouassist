using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.QualityAssurance.Aggregates;
using NouAssist.RequirementsManagement.QualityAssurance.Constants;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.QualityAssurance.EntityFrameworkCore.Configurations
{
    public class DefectReportConfiguration : IEntityTypeConfiguration<DefectReport>
    {
        public void Configure(EntityTypeBuilder<DefectReport> builder)
        {
            // Apply ABP base conventions
            builder.ConfigureByConvention();

            // Table Mapping
            builder.ToTable(QADbProperties.DbTablePrefix + "Defects", QADbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(QADomainConstants.MaxDefectTitleLength);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(QADomainConstants.MaxDefectDescriptionLength);

            builder.Property(x => x.StepsToReproduce)
                .IsRequired()
                .HasMaxLength(QADomainConstants.MaxStepsToReproduceLength);

            builder.Property(x => x.ExpectedOutcome)
                .IsRequired()
                .HasMaxLength(QADomainConstants.MaxExpectedOutcomeLength);

            builder.Property(x => x.ActualOutcome)
                .IsRequired()
                .HasMaxLength(QADomainConstants.MaxActualOutcomeLength);

            // Enum Mappings
            builder.Property(x => x.Severity)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Priority)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Foreign keys and references
            builder.Property(x => x.AssigneeId)
                .IsRequired(false);

            builder.Property(x => x.TestRunId)
                .IsRequired(false);

            builder.Property(x => x.TestCycleId)
                .IsRequired(false);

            builder.Property(x => x.TestPlanId)
                .IsRequired(false);

            // Other properties
            builder.Property(x => x.Environment)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Version)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.FixCommitId)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(x => x.VerifierId)
                .IsRequired(false);

            builder.Property(x => x.FixedInVersion)
                .IsRequired(false)
                .HasMaxLength(100);

            // Timestamps
            builder.Property(x => x.AssignedTime)
                .IsRequired(false);

            builder.Property(x => x.FixedTime)
                .IsRequired(false);

            builder.Property(x => x.VerifiedTime)
                .IsRequired(false);

            builder.Property(x => x.ClosedTime)
                .IsRequired(false);

            // Comments owned collection
            builder.OwnsMany(x => x.Comments, commentsBuilder =>
            {
                commentsBuilder.ToTable(QADbProperties.DbTablePrefix + "DefectComments", QADbProperties.DbSchema);
                
                commentsBuilder.WithOwner().HasForeignKey("DefectReportId");
                
                commentsBuilder.Property<int>("Id")
                    .ValueGeneratedOnAdd();
                
                commentsBuilder.HasKey("Id", "DefectReportId");
                
                commentsBuilder.Property(c => c.Text)
                    .IsRequired()
                    .HasMaxLength(QADomainConstants.MaxCommentLength);
                
                commentsBuilder.Property(c => c.CreationTime)
                    .IsRequired();
                
                commentsBuilder.Property(c => c.CreatorId)
                    .IsRequired(false);
            });

            // Affected requirements collection
            builder.Property(x => x.AffectedRequirementIds)
                .HasColumnName("AffectedRequirementIds")
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(id => Guid.Parse(id))
                        .ToList());

            // Indexes
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.Severity);
            builder.HasIndex(x => x.Priority);
            builder.HasIndex(x => x.AssigneeId);
            builder.HasIndex(x => x.TestCycleId);
            builder.HasIndex(x => x.TestPlanId);
            builder.HasIndex(x => x.Version);
        }
    }
}
