using FluentValidation;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Constants;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialModels.Dtos;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Validation.FinancialModel
{
    public class CreateFinancialModelDtoValidator : AbstractValidator<CreateFinancialModelDto>
    {
        public CreateFinancialModelDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Model name is required")
                .MaximumLength(PerformanceFinancialModelingConsts.MaxModelNameLength)
                .WithMessage($"Model name cannot exceed {PerformanceFinancialModelingConsts.MaxModelNameLength} characters");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Model description is required")
                .MaximumLength(PerformanceFinancialModelingConsts.MaxModelDescriptionLength)
                .WithMessage($"Model description cannot exceed {PerformanceFinancialModelingConsts.MaxModelDescriptionLength} characters");

            RuleFor(x => x.ProjectionYears)
                .GreaterThan(0)
                .WithMessage("Projection years must be greater than 0")
                .LessThanOrEqualTo(PerformanceFinancialModelingConsts.MaxProjectionYears)
                .WithMessage($"Projection years cannot exceed {PerformanceFinancialModelingConsts.MaxProjectionYears}");

            RuleFor(x => x.BaseDate)
                .NotEmpty()
                .WithMessage("Base date is required");

            RuleFor(x => x.Currency)
                .MaximumLength(3)
                .When(x => !string.IsNullOrEmpty(x.Currency))
                .WithMessage("Currency code cannot exceed 3 characters");
        }
    }
}
