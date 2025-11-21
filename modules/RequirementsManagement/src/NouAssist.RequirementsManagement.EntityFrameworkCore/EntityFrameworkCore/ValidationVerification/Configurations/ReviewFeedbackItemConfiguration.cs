using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.ValidationVerification.Constants;
using NouAssist.RequirementsManagement.ValidationVerification.Entities;
using NouAssist.RequirementsManagement.ValidationVerification.ValueObjects;

namespace NouAssist.RequirementsManagement.ValidationVerification.EntityFrameworkCore.Configurations
{
    public class ReviewFeedbackItemConfiguration : IEntityTypeConfiguration<ReviewFeedbackItem>
    {
        public void Configure(EntityTypeBuilder<ReviewFeedbackItem> builder)
        {
            // Table Mapping
            builder.ToTable(ValidationVerificationDbProperties.DbTablePrefix + "ReviewFeedbackItems", 
                            ValidationVerificationDbProperties.DbSchema);

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.ReviewCycleId)
                .IsRequired();

            builder.Property(x => x.ReviewerId)
                .IsRequired();

            builder.Property(x => x.SubmittedAt)
                .IsRequired();

            builder.Property(x => x.ReviewerName)
                .HasMaxLength(256);

            // Value Object Mapping - ReviewDecision
            builder.OwnsOne(x => x.Decision, decisionBuilder => 
            {
                decisionBuilder.Property(d => d.DecisionType)
                    .HasColumnName("DecisionType")
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();
                    
                decisionBuilder.Property(d => d.Reason)
                    .HasColumnName("DecisionReason")
                    .HasMaxLength(ValidationVerificationConsts.ReviewFeedback.MaxReasonLength);
                    
                decisionBuilder.Property(d => d.DecisionDate)
                    .HasColumnName("DecisionDate")
                    .IsRequired();
                    
                decisionBuilder.Property(d => d.ReviewerId)
                    .HasColumnName("DecisionReviewerId")
                    .IsRequired();
            });

            // Value Object Mapping - FeedbackComment (optional)
            builder.OwnsOne(x => x.Comment, commentBuilder => 
            {
                commentBuilder.Property(c => c.Comment)
                    .HasColumnName("FeedbackComment")
                    .HasMaxLength(ValidationVerificationConsts.ReviewFeedback.MaxCommentLength);
                    
                commentBuilder.Property(c => c.CreatedAt)
                    .HasColumnName("CommentCreatedAt");
                    
                commentBuilder.Property(c => c.AuthorId)
                    .HasColumnName("CommentAuthorId");
                    
                commentBuilder.Property(c => c.AuthorName)
                    .HasColumnName("CommentAuthorName")
                    .HasMaxLength(256);
                    
                commentBuilder.Property(c => c.IsInternal)
                    .HasColumnName("CommentIsInternal");
            });

            // Indexing
            builder.HasIndex(x => x.ReviewCycleId);
            builder.HasIndex(x => x.ReviewerId);
            builder.HasIndex(x => x.SubmittedAt);
        }
    }
}
