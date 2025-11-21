using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.ValidationVerification.Constants;
using NouAssist.RequirementsManagement.ValidationVerification.Entities;
using NouAssist.RequirementsManagement.ValidationVerification.ValueObjects;

namespace NouAssist.RequirementsManagement.ValidationVerification.EntityFrameworkCore.Configurations
{
    public class ReviewWorkflowInstanceConfiguration : IEntityTypeConfiguration<ReviewWorkflowInstance>
    {
        public void Configure(EntityTypeBuilder<ReviewWorkflowInstance> builder)
        {
            // Table Mapping
            builder.ToTable(ValidationVerificationDbProperties.DbTablePrefix + "ReviewWorkflowInstances", 
                            ValidationVerificationDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.ReviewCycleId)
                .IsRequired();

            // Value Object Mapping - WorkflowDefinitionRef
            builder.OwnsOne(x => x.WorkflowDefinition, workflowBuilder => 
            {
                workflowBuilder.Property(w => w.Name)
                    .HasColumnName("WorkflowDefinitionName")
                    .HasMaxLength(ValidationVerificationConsts.WorkflowDefinition.MaxNameLength)
                    .IsRequired();
                    
                workflowBuilder.Property(w => w.Version)
                    .HasColumnName("WorkflowDefinitionVersion")
                    .HasMaxLength(ValidationVerificationConsts.WorkflowDefinition.MaxVersionLength)
                    .IsRequired();
                    
                workflowBuilder.Property(w => w.DefinitionId)
                    .HasColumnName("WorkflowDefinitionId")
                    .IsRequired();
            });

            builder.Property(x => x.StartedAt)
                .IsRequired();

            builder.Property(x => x.CompletedAt)
                .IsRequired(false);

            // Enum mapping
            builder.Property(x => x.FinalStatus)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(x => x.CompletionReason)
                .HasMaxLength(1000);

            builder.Property(x => x.WorkflowState)
                .HasMaxLength(100);

            builder.Property(x => x.WorkflowContext)
                .HasColumnType("jsonb");

            // Indexing
            builder.HasIndex(x => x.ReviewCycleId).IsUnique();
            builder.HasIndex(x => x.StartedAt);
        }
    }
}
