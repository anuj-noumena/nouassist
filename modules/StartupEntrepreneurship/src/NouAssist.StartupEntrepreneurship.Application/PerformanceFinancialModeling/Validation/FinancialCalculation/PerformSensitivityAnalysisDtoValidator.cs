using FluentValidation;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.FinancialCalculations.Dtos;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Validation.FinancialCalculation
{
    public class PerformSensitivityAnalysisDtoValidator : AbstractValidator<PerformSensitivityAnalysisDto>
    {
        public PerformSensitivityAnalysisDtoValidator()
        {
            RuleFor(x => x.VariableToAnalyze)
                .NotEmpty()
                .WithMessage("Variable to analyze is required");

            RuleFor(x => x.VariancePercentage)
                .GreaterThan(0)
                .WithMessage("Variance percentage must be greater than 0")
                .LessThanOrEqualTo(100)
                .WithMessage("Variance percentage cannot exceed 100");

            RuleFor(x => x.Notes)
                .MaximumLength(PerformanceFinancialModelingConsts.MaxNotesLength)
                .When(x => !string.IsNullOrEmpty(x.Notes))
                .WithMessage($"Notes cannot exceed {PerformanceFinancialModelingConsts.MaxNotesLength} characters");
        }
    }
}
