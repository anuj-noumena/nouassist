using FluentValidation;
using NouAssist.RequirementsManagement.Specification.Constants;
using NouAssist.RequirementsManagement.Specification.Requirements.Dtos;

namespace NouAssist.RequirementsManagement.Specification.Validation.Requirements
{
    public class UpdateRequirementDtoValidator : AbstractValidator<UpdateRequirementDto>
    {
        public UpdateRequirementDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required")
                .MaximumLength(RequirementSpecificationManagementConsts.Requirement.MaxTitleLength)
                .WithMessage($"Title cannot exceed {RequirementSpecificationManagementConsts.Requirement.MaxTitleLength} characters");

            RuleFor(x => x.Description)
                .MaximumLength(RequirementSpecificationManagementConsts.Requirement.MaxDescriptionLength)
                .WithMessage($"Description cannot exceed {RequirementSpecificationManagementConsts.Requirement.MaxDescriptionLength} characters");

            RuleFor(x => x.Priority)
                .IsInEnum()
                .WithMessage("Invalid requirement priority");
        }
    }
}
