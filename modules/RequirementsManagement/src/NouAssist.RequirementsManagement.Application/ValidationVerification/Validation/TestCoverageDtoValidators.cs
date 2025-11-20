using FluentValidation;
using NouAssist.RequirementsManagement.ValidationVerification.TestCoverage.Dtos;

namespace NouAssist.RequirementsManagement.ValidationVerification.Validation
{
    /// <summary>
    /// Validator for GenerateCoverageReportDto
    /// </summary>
    public class GenerateCoverageReportDtoValidator : AbstractValidator<GenerateCoverageReportDto>
    {
        public GenerateCoverageReportDtoValidator()
        {
            RuleFor(x => x.RequirementIds)
                .NotEmpty()
                .WithMessage("At least one requirement must be specified");

            RuleFor(x => x.ReportScope)
                .MaximumLength(500);
        }
    }

    /// <summary>
    /// Validator for IdentifyTestGapsDto
    /// </summary>
    public class IdentifyTestGapsDtoValidator : AbstractValidator<IdentifyTestGapsDto>
    {
        public IdentifyTestGapsDtoValidator()
        {
            RuleFor(x => x.RequirementIds)
                .NotEmpty()
                .WithMessage("At least one requirement must be specified");

            RuleFor(x => x.MinimumCoverageThreshold)
                .InclusiveBetween(0.0, 1.0)
                .WithMessage("Coverage threshold must be between 0 and 1");
        }
    }
}
