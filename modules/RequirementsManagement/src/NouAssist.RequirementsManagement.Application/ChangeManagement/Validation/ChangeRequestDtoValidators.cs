using FluentValidation;
using NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos;
using NouAssist.RequirementsManagement.ChangeManagement.Constants;
using NouAssist.RequirementsManagement.ChangeManagement.Enums;

namespace NouAssist.RequirementsManagement.ChangeManagement.Validation
{
    public class CreateChangeRequestDtoValidator : AbstractValidator<CreateChangeRequestDto>
    {
        public CreateChangeRequestDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(ChangeManagementConsts.MaxTitleLength)
                .WithMessage($"Title cannot exceed {ChangeManagementConsts.MaxTitleLength} characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(ChangeManagementConsts.MaxDescriptionLength)
                .WithMessage($"Description cannot exceed {ChangeManagementConsts.MaxDescriptionLength} characters.");

            RuleFor(x => x.AffectedRequirementId)
                .NotEmpty().WithMessage("Affected requirement ID is required.");

            RuleFor(x => x.AffectedRequirementVersion)
                .NotEmpty().WithMessage("Affected requirement version is required.")
                .MaximumLength(50).WithMessage("Version cannot exceed 50 characters.");

            RuleForEach(x => x.Rationales).SetValidator(new CreateChangeRationaleDtoValidator());
        }
    }

    public class CreateChangeRationaleDtoValidator : AbstractValidator<CreateChangeRationaleDto>
    {
        public CreateChangeRationaleDtoValidator()
        {
            RuleFor(x => x.RationaleText)
                .NotEmpty().WithMessage("Rationale text is required.")
                .MaximumLength(ChangeManagementConsts.MaxRationaleLength)
                .WithMessage($"Rationale text cannot exceed {ChangeManagementConsts.MaxRationaleLength} characters.");

            RuleFor(x => x.Category)
                .MaximumLength(100).WithMessage("Category cannot exceed 100 characters.");
        }
    }

    public class UpdateChangeRequestDtoValidator : AbstractValidator<UpdateChangeRequestDto>
    {
        public UpdateChangeRequestDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(ChangeManagementConsts.MaxTitleLength)
                .WithMessage($"Title cannot exceed {ChangeManagementConsts.MaxTitleLength} characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(ChangeManagementConsts.MaxDescriptionLength)
                .WithMessage($"Description cannot exceed {ChangeManagementConsts.MaxDescriptionLength} characters.");
        }
    }

    public class AddImpactStatementDtoValidator : AbstractValidator<AddImpactStatementDto>
    {
        public AddImpactStatementDtoValidator()
        {
            RuleFor(x => x.ImpactType)
                .IsInEnum().WithMessage("Invalid impact type.");

            RuleFor(x => x.CostImpact)
                .GreaterThanOrEqualTo(0).When(x => x.CostImpact.HasValue)
                .WithMessage("Cost impact must be a positive value or zero.");

            RuleFor(x => x.TimeImpactDays)
                .GreaterThanOrEqualTo(0).When(x => x.TimeImpactDays.HasValue)
                .WithMessage("Time impact must be a positive number of days or zero.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Impact description is required.")
                .MaximumLength(ChangeManagementConsts.MaxDescriptionLength)
                .WithMessage($"Description cannot exceed {ChangeManagementConsts.MaxDescriptionLength} characters.");

            RuleFor(x => x.Dependencies)
                .MaximumLength(1000).WithMessage("Dependencies cannot exceed 1000 characters.");
        }
    }

    public class ApprovalDecisionDtoValidator : AbstractValidator<ApprovalDecisionDto>
    {
        public ApprovalDecisionDtoValidator()
        {
            RuleFor(x => x.ApprovalStepId)
                .NotEmpty().WithMessage("Approval step ID is required.");

            RuleFor(x => x.Decision)
                .IsInEnum().WithMessage("Invalid approval decision.")
                .NotEqual(ApprovalDecision.Pending).WithMessage("Decision cannot be Pending.");

            RuleFor(x => x.DecisionReason)
                .NotEmpty().WithMessage("Decision reason is required.")
                .MaximumLength(ChangeManagementConsts.MaxDecisionReasonLength)
                .WithMessage($"Decision reason cannot exceed {ChangeManagementConsts.MaxDecisionReasonLength} characters.");
        }
    }

    public class ChangeStatusDtoValidator : AbstractValidator<ChangeStatusDto>
    {
        public ChangeStatusDtoValidator()
        {
            RuleFor(x => x.TargetStatus)
                .IsInEnum().WithMessage("Invalid target status.");

            RuleFor(x => x.Reason)
                .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters.");
        }
    }
}
