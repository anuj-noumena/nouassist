using FluentValidation;
using NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.TrackedMetrics.Dtos;

namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Validation.TrackedMetric
{
    public class CreateTrackedMetricDtoValidator : AbstractValidator<CreateTrackedMetricDto>
    {
        public CreateTrackedMetricDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Metric name is required")
                .MaximumLength(PerformanceFinancialModelingConsts.MaxMetricNameLength)
                .WithMessage($"Metric name cannot exceed {PerformanceFinancialModelingConsts.MaxMetricNameLength} characters");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Metric description is required")
                .MaximumLength(PerformanceFinancialModelingConsts.MaxMetricDescriptionLength)
                .WithMessage($"Metric description cannot exceed {PerformanceFinancialModelingConsts.MaxMetricDescriptionLength} characters");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid metric type");

            RuleFor(x => x.DataType)
                .IsInEnum()
                .WithMessage("Invalid data type");

            RuleFor(x => x.Frequency)
                .IsInEnum()
                .WithMessage("Invalid frequency");

            RuleFor(x => x.Unit)
                .MaximumLength(PerformanceFinancialModelingConsts.MaxMetricUnitLength)
                .When(x => !string.IsNullOrEmpty(x.Unit))
                .WithMessage($"Unit cannot exceed {PerformanceFinancialModelingConsts.MaxMetricUnitLength} characters");

            RuleFor(x => x.DefinitionId)
                .NotEmpty()
                .WithMessage("Metric definition is required");
        }
    }
}
