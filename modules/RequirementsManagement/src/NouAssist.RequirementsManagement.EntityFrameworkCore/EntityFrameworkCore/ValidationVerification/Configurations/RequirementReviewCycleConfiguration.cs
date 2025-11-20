using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.ValidationVerification.Aggregates;
using NouAssist.RequirementsManagement.ValidationVerification.Entities;
using NouAssist.RequirementsManagement.ValidationVerification.ValueObjects;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.ValidationVerification.EntityFrameworkCore.Configurations
{
    public class RequirementReviewCycleConfiguration : IEntityTypeConfiguration<RequirementReviewCycle>
    {
        public void Configure(EntityTypeBuilder<RequirementReviewCycle> builder)
        {
            // Apply ABP base conventions (auditing, soft delete, multi-tenancy, concurrency stamp)
            builder.ConfigureByConvention(); 

            // Table Mapping
            builder.ToTable(ValidationVerificationDbProperties.DbTablePrefix + "RequirementReviewCycles", 
                            ValidationVerificationDbProperties.DbSchema);

            // Primary Key (handled by AggregateRoot<TKey>)
            builder.HasKey(x => x.Id);

            // Property Mappings
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(ValidationVerificationConsts.RequirementReviewCycle.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(ValidationVerificationConsts.RequirementReviewCycle.MaxDescriptionLength);

            builder.Property(x => x.RequirementId)
                .IsRequired();

            // Enum Mapping
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

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

            // DateTime properties
            builder.Property(x => x.TargetCompletionDate)
                .IsRequired(false);

            builder.Property(x => x.ActualCompletionDate)
                .IsRequired(false);

            builder.Property(x => x.CompletionComments)
                .HasMaxLength(ValidationVerificationConsts.RequirementReviewCycle.MaxCommentsLength);

            // Collection Mapping - ReviewerAssignments (Owned Entities)
            builder.OwnsMany(x => x.ReviewerAssignments, assignmentBuilder =>
            {
                assignmentBuilder.ToTable(ValidationVerificationDbProperties.DbTablePrefix + "ReviewerAssignments",
                                          ValidationVerificationDbProperties.DbSchema);
                                          
                assignmentBuilder.WithOwner().HasForeignKey("RequirementReviewCycleId");
                assignmentBuilder.Property<Guid>("Id");
                assignmentBuilder.HasKey("Id");

                assignmentBuilder.Property(a => a.ReviewerId).IsRequired();
                assignmentBuilder.Property(a => a.ReviewerName).IsRequired().HasMaxLength(256);
                assignmentBuilder.Property(a => a.ReviewerEmail).IsRequired().HasMaxLength(256);
                assignmentBuilder.Property(a => a.AssignedAt).IsRequired();
                assignmentBuilder.Property(a => a.DueDate).IsRequired(false);
                assignmentBuilder.Property(a => a.IsRequired).IsRequired();
            });

            // Navigation Property - WorkflowInstance
            builder.HasOne(x => x.WorkflowInstance)
                   .WithOne()
                   .HasForeignKey<ReviewWorkflowInstance>(wi => wi.ReviewCycleId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Cascade);

            // Navigation Property - FeedbackItems
            builder.HasMany(x => x.FeedbackItems)
                   .WithOne()
                   .HasForeignKey(f => f.ReviewCycleId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexing
            builder.HasIndex(x => x.RequirementId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.TargetCompletionDate);

            // Extra Properties (ABP handles JSON mapping by default)
            builder.Property(x => x.ExtraProperties).HasColumnName(nameof(RequirementReviewCycle.ExtraProperties));

            // Concurrency Stamp (Handled by ConfigureByConvention)
            builder.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
