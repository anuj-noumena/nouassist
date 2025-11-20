using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.ValidationVerification.Entities;
using NouAssist.RequirementsManagement.ValidationVerification.ValueObjects;

namespace NouAssist.RequirementsManagement.ValidationVerification.EntityFrameworkCore.Configurations
{
    public class TestStepConfiguration : IEntityTypeConfiguration<TestStep>
    {
        public void Configure(EntityTypeBuilder<TestStep> builder)
        {
            // Table Mapping
            builder.ToTable(ValidationVerificationDbProperties.DbTablePrefix + "TestSteps", 
                            ValidationVerificationDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.TestCaseId)
                .IsRequired();

            builder.Property(x => x.StepNumber)
                .IsRequired();

            // Enum mapping
            builder.Property(x => x.ExecutionStatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.Comments)
                .HasMaxLength(ValidationVerificationConsts.TestStep.MaxCommentsLength);

            // Value Object Mapping - TestStepInstruction
            builder.OwnsOne(x => x.Instruction, instructionBuilder => 
            {
                instructionBuilder.Property(i => i.Instruction)
                    .HasColumnName("StepInstruction")
                    .HasMaxLength(ValidationVerificationConsts.TestStep.MaxInstructionLength)
                    .IsRequired();
                    
                instructionBuilder.Property(i => i.TestData)
                    .HasColumnName("StepTestData");
                    
                instructionBuilder.Property(i => i.Prerequisites)
                    .HasColumnName("StepPrerequisites");
            });

            // Value Object Mapping - ExpectedResult
            builder.OwnsOne(x => x.ExpectedResult, expectedBuilder => 
            {
                expectedBuilder.Property(e => e.Description)
                    .HasColumnName("ExpectedResultDescription")
                    .HasMaxLength(ValidationVerificationConsts.TestStep.MaxExpectedResultLength)
                    .IsRequired();
                    
                expectedBuilder.Property(e => e.AcceptanceCriteria)
                    .HasColumnName("ExpectedResultAcceptanceCriteria");
            });

            // Value Object Mapping - ActualResult (optional)
            builder.OwnsOne(x => x.ActualResult, actualBuilder => 
            {
                actualBuilder.Property(a => a.Description)
                    .HasColumnName("ActualResultDescription")
                    .HasMaxLength(ValidationVerificationConsts.TestStep.MaxActualResultLength);
                    
                actualBuilder.Property(a => a.Evidence)
                    .HasColumnName("ActualResultEvidence");
                    
                actualBuilder.Property(a => a.ExecutedAt)
                    .HasColumnName("ActualResultExecutedAt");
                    
                actualBuilder.Property(a => a.ExecutedBy)
                    .HasColumnName("ActualResultExecutedBy");
            });

            // Indexing
            builder.HasIndex(x => x.TestCaseId);
            builder.HasIndex(x => new { x.TestCaseId, x.StepNumber }).IsUnique();
            builder.HasIndex(x => x.ExecutionStatus);
        }
    }
}
