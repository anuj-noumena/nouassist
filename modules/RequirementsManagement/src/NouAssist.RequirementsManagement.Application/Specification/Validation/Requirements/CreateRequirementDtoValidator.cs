using FluentValidation;
using NouAssist.RequirementsManagement.Specification.Constants;
using NouAssist.RequirementsManagement.Specification.Requirements.Dtos;

namespace NouAssist.RequirementsManagement.Specification.Validation.Requirements
{
    public class CreateRequirementDtoValidator : AbstractValidator<CreateRequirementDto>
    {
        public CreateRequirementDtoValidator()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty()
                .WithMessage("Identifier is required")
                .MaximumLength(RequirementSpecificationManagementConsts.Requirement.MaxIdentifierLength)
                .WithMessage($"Identifier cannot exceed {RequirementSpecificationManagementConsts.Requirement.MaxIdentifierLength} characters")
                .Matches(@"^[A-Za-z0-9_-]+$")
                .WithMessage("Identifier can only contain letters, numbers, hyphens, and underscores");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required")
                .MaximumLength(RequirementSpecificationManagementConsts.Requirement.MaxTitleLength)
                .WithMessage($"Title cannot exceed {RequirementSpecificationManagementConsts.Requirement.MaxTitleLength} characters");

            RuleFor(x => x.Description)
                .MaximumLength(RequirementSpecificationManagementConsts.Requirement.MaxDescriptionLength)
                .WithMessage($"Description cannot exceed {RequirementSpecificationManagementConsts.Requirement.MaxDescriptionLength} characters");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid requirement type");

            RuleFor(x => x.Priority)
                .IsInEnum()
                .WithMessage("Invalid requirement priority");
        }
    }
}
