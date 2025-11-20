using FluentValidation;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas.Dtos;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Validation
{
    public class CreateBusinessModelCanvasDtoValidator : AbstractValidator<CreateBusinessModelCanvasDto>
    {
        public CreateBusinessModelCanvasDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(BusinessModelManagementConsts.BusinessModelCanvas.MaxTitleLength)
                .WithMessage("Title must be between 1 and {MaxLength} characters");

            RuleFor(x => x.Description)
                .MaximumLength(BusinessModelManagementConsts.BusinessModelCanvas.MaxDescriptionLength)
                .WithMessage("Description cannot exceed {MaxLength} characters");
        }
    }

    public class UpdateBusinessModelCanvasDtoValidator : AbstractValidator<UpdateBusinessModelCanvasDto>
    {
        public UpdateBusinessModelCanvasDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(BusinessModelManagementConsts.BusinessModelCanvas.MaxTitleLength);

            RuleFor(x => x.Description)
                .MaximumLength(BusinessModelManagementConsts.BusinessModelCanvas.MaxDescriptionLength);
        }
    }

    public class CreateHypothesisDtoValidator : AbstractValidator<CreateHypothesisDto>
    {
        public CreateHypothesisDtoValidator()
        {
            RuleFor(x => x.ComponentType)
                .IsInEnum()
                .WithMessage("Invalid component type");

            RuleFor(x => x.Assumption)
                .NotEmpty()
                .MaximumLength(BusinessModelManagementConsts.Hypothesis.MaxAssumptionLength)
                .WithMessage("Assumption must be between 1 and {MaxLength} characters");
        }
    }

    public class ValidateHypothesisDtoValidator : AbstractValidator<ValidateHypothesisDto>
    {
        public ValidateHypothesisDtoValidator()
        {
            RuleFor(x => x.TestDescription)
                .NotEmpty()
                .MaximumLength(BusinessModelManagementConsts.Hypothesis.MaxTestDescriptionLength);

            RuleFor(x => x.TestResult)
                .NotEmpty()
                .MaximumLength(BusinessModelManagementConsts.Hypothesis.MaxResultDescriptionLength);

            RuleFor(x => x.FinalStatus)
                .IsInEnum()
                .Must(status => status != Enums.ValidationStatus.NotTested && status != Enums.ValidationStatus.InProgress)
                .WithMessage("Final status must be Validated, Invalidated, or NeedsMoreTesting");

            RuleFor(x => x.TestEndDate)
                .GreaterThanOrEqualTo(x => x.TestStartDate)
                .When(x => x.TestStartDate.HasValue && x.TestEndDate.HasValue)
                .WithMessage("Test end date must be after test start date");
        }
    }
}
