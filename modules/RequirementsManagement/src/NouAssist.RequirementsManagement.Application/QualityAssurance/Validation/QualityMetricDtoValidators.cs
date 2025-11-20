using FluentValidation;
using NouAssist.RequirementsManagement.QualityAssurance.QualityMetrics.Dtos;

namespace NouAssist.RequirementsManagement.QualityAssurance.Validation
{
    public class CreateQualityMetricDtoValidator : AbstractValidator<CreateQualityMetricDto>
    {
        public CreateQualityMetricDtoValidator()
        {
            RuleFor(x => x.Unit).NotEmpty().WithMessage("Unit of measurement is required");
            
            // At least one of TestPlanId or ReleaseVersion must be provided
            RuleFor(x => new { x.TestPlanId, x.ReleaseVersion })
                .Must(x => x.TestPlanId.HasValue || !string.IsNullOrEmpty(x.ReleaseVersion))
                .WithMessage("Either TestPlanId or ReleaseVersion must be provided");
        }
    }
}
