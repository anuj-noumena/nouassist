using FluentValidation;
using NouAssist.RequirementsManagement.QualityAssurance.Defects.Dtos;

namespace NouAssist.RequirementsManagement.QualityAssurance.Validation
{
    public class CreateDefectDtoValidator : AbstractValidator<CreateDefectDto>
    {
        public CreateDefectDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Defect title is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Defect description is required");
            RuleFor(x => x.StepsToReproduce).NotEmpty().WithMessage("Steps to reproduce are required");
            RuleFor(x => x.ExpectedOutcome).NotEmpty().WithMessage("Expected outcome is required");
            RuleFor(x => x.ActualOutcome).NotEmpty().WithMessage("Actual outcome is required");
            RuleFor(x => x.Environment).NotEmpty().WithMessage("Environment is required");
            RuleFor(x => x.Version).NotEmpty().WithMessage("Version is required");
        }
    }

    public class UpdateDefectDtoValidator : AbstractValidator<UpdateDefectDto>
    {
        public UpdateDefectDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Defect title is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Defect description is required");
            RuleFor(x => x.StepsToReproduce).NotEmpty().WithMessage("Steps to reproduce are required");
            RuleFor(x => x.ExpectedOutcome).NotEmpty().WithMessage("Expected outcome is required");
            RuleFor(x => x.ActualOutcome).NotEmpty().WithMessage("Actual outcome is required");
            RuleFor(x => x.Environment).NotEmpty().WithMessage("Environment is required");
            RuleFor(x => x.Version).NotEmpty().WithMessage("Version is required");
        }
    }

    public class AddDefectCommentDtoValidator : AbstractValidator<AddDefectCommentDto>
    {
        public AddDefectCommentDtoValidator()
        {
            RuleFor(x => x.DefectId).NotEmpty().WithMessage("Defect ID is required");
            RuleFor(x => x.Text).NotEmpty().WithMessage("Comment text is required");
        }
    }
}
