using FluentValidation;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivot.Dtos;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Validation
{
    public class CreateStrategicPivotDtoValidator : AbstractValidator<CreateStrategicPivotDto>
    {
        public CreateStrategicPivotDtoValidator()
        {
            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid pivot type");

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(BusinessModelManagementConsts.StrategicPivot.MaxDescriptionLength);

            RuleFor(x => x.TargetCompletionDate)
                .GreaterThan(DateTime.Now)
                .WithMessage("Target completion date must be in the future");

            RuleFor(x => x.Rationale)
                .NotNull()
                .SetValidator(new PivotRationaleDtoValidator());
        }
    }

    public class PivotRationaleDtoValidator : AbstractValidator<PivotRationaleDto>
    {
        public PivotRationaleDtoValidator()
        {
            RuleFor(x => x.Reason)
                .NotEmpty()
                .MaximumLength(BusinessModelManagementConsts.StrategicPivot.MaxReasonLength);

            RuleFor(x => x.ExpectedOutcome)
                .NotEmpty()
                .MaximumLength(BusinessModelManagementConsts.StrategicPivot.MaxExpectedOutcomeLength);

            RuleFor(x => x.KeyInsights)
                .NotNull()
                .Must(insights => insights?.Count > 0)
                .WithMessage("At least one key insight is required");

            RuleFor(x => x.SuccessMetrics)
                .NotNull()
                .Must(metrics => metrics?.Count > 0)
                .WithMessage("At least one success metric is required");
        }
    }

    public class CreateImpactMetricDtoValidator : AbstractValidator<CreateImpactMetricDto>
    {
        public CreateImpactMetricDtoValidator()
        {
            RuleFor(x => x.MetricName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Unit)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.TargetValue)
                .GreaterThan(x => x.BaselineValue)
                .When(x => x.BaselineValue.HasValue && x.TargetValue.HasValue)
                .WithMessage("Target value must be greater than baseline value");
        }
    }

    public class RecordMeasurementDtoValidator : AbstractValidator<RecordMeasurementDto>
    {
        public RecordMeasurementDtoValidator()
        {
            RuleFor(x => x.MeasuredAt)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Measurement date cannot be in the future");

            RuleFor(x => x.Notes)
                .MaximumLength(1000);
        }
    }
}
