using FluentValidation;
using NouAssist.RequirementsManagement.Specification.Constants;
using NouAssist.RequirementsManagement.Specification.Requirements.Dtos;

namespace NouAssist.RequirementsManagement.Specification.Validation.Requirements
{
    public class CreateAcceptanceCriterionDtoValidator : AbstractValidator<CreateAcceptanceCriterionDto>
    {
        public CreateAcceptanceCriterionDtoValidator()
        {
            RuleFor(x => x.Statement)
                .NotEmpty()
                .WithMessage("Statement is required")
                .MaximumLength(RequirementSpecificationManagementConsts.AcceptanceCriterion.MaxStatementLength)
                .WithMessage($"Statement cannot exceed {RequirementSpecificationManagementConsts.AcceptanceCriterion.MaxStatementLength} characters");

            RuleFor(x => x.Description)
                .MaximumLength(RequirementSpecificationManagementConsts.AcceptanceCriterion.MaxDescriptionLength)
                .WithMessage($"Description cannot exceed {RequirementSpecificationManagementConsts.AcceptanceCriterion.MaxDescriptionLength} characters");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid criterion type");
        }
    }
}
