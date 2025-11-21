using System;
using FluentValidation;
using NouAssist.RequirementsManagement.ValidationVerification.Constants;
using NouAssist.RequirementsManagement.ValidationVerification.Enums;
using NouAssist.RequirementsManagement.ValidationVerification.ReviewCycles.Dtos;

namespace NouAssist.RequirementsManagement.ValidationVerification.Validation
{
    /// <summary>
    /// Validator for CreateReviewCycleDto
    /// </summary>
    public class CreateReviewCycleDtoValidator : AbstractValidator<CreateReviewCycleDto>
    {
        public CreateReviewCycleDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(ValidationVerificationConsts.RequirementReviewCycle.MaxNameLength);

            RuleFor(x => x.Description)
                .MaximumLength(ValidationVerificationConsts.RequirementReviewCycle.MaxDescriptionLength);

            RuleFor(x => x.RequirementId)
                .NotEmpty();

            RuleFor(x => x.WorkflowDefinition)
                .NotNull()
                .SetValidator(new CreateWorkflowDefinitionDtoValidator());

            RuleFor(x => x.TargetCompletionDate)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.TargetCompletionDate.HasValue)
                .WithMessage("Target completion date must be in the future");

            RuleFor(x => x.ReviewerAssignments)
                .NotEmpty()
                .WithMessage("At least one reviewer must be assigned");

            RuleForEach(x => x.ReviewerAssignments)
                .SetValidator(new CreateReviewerAssignmentDtoValidator());
        }
    }

    /// <summary>
    /// Validator for CreateWorkflowDefinitionDto
    /// </summary>
    public class CreateWorkflowDefinitionDtoValidator : AbstractValidator<CreateWorkflowDefinitionDto>
    {
        public CreateWorkflowDefinitionDtoValidator()
        {
            RuleFor(x => x.DefinitionId)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(ValidationVerificationConsts.WorkflowDefinition.MaxNameLength);

            RuleFor(x => x.Version)
                .NotEmpty()
                .MaximumLength(ValidationVerificationConsts.WorkflowDefinition.MaxVersionLength);
        }
    }

    /// <summary>
    /// Validator for CreateReviewerAssignmentDto
    /// </summary>
    public class CreateReviewerAssignmentDtoValidator : AbstractValidator<CreateReviewerAssignmentDto>
    {
        public CreateReviewerAssignmentDtoValidator()
        {
            RuleFor(x => x.ReviewerId)
                .NotEmpty();

            RuleFor(x => x.ReviewerName)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.ReviewerEmail)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256);

            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.DueDate.HasValue)
                .WithMessage("Due date must be in the future");
        }
    }

    /// <summary>
    /// Validator for UpdateReviewCycleDto
    /// </summary>
    public class UpdateReviewCycleDtoValidator : AbstractValidator<UpdateReviewCycleDto>
    {
        public UpdateReviewCycleDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(ValidationVerificationConsts.RequirementReviewCycle.MaxNameLength);

            RuleFor(x => x.Description)
                .MaximumLength(ValidationVerificationConsts.RequirementReviewCycle.MaxDescriptionLength);

            RuleFor(x => x.TargetCompletionDate)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.TargetCompletionDate.HasValue)
                .WithMessage("Target completion date must be in the future");
        }
    }

    /// <summary>
    /// Validator for SubmitReviewDecisionDto
    /// </summary>
    public class SubmitReviewDecisionDtoValidator : AbstractValidator<SubmitReviewDecisionDto>
    {
        public SubmitReviewDecisionDtoValidator()
        {
            RuleFor(x => x.DecisionType)
                .IsInEnum();

            RuleFor(x => x.Reason)
                .MaximumLength(ValidationVerificationConsts.ReviewFeedback.MaxReasonLength);

            RuleFor(x => x.Comment)
                .MaximumLength(ValidationVerificationConsts.ReviewFeedback.MaxCommentLength);

            // Require reason for reject and request changes decisions
            RuleFor(x => x.Reason)
                .NotEmpty()
                .When(x => x.DecisionType == ReviewDecisionType.Reject || 
                          x.DecisionType == ReviewDecisionType.RequestChanges)
                .WithMessage("Reason is required for reject and request changes decisions");
        }
    }
}
