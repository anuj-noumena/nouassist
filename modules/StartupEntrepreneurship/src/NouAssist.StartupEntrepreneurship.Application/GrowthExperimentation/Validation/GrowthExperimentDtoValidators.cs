using FluentValidation;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Validation
{
    public class CreateGrowthExperimentDtoValidator : AbstractValidator<CreateGrowthExperimentDto>
    {
        public CreateGrowthExperimentDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(128)
                .WithMessage("Experiment name is required and must be at most 128 characters.");

            RuleFor(x => x.Goal).NotNull();
            RuleFor(x => x.Goal.Description).NotEmpty().When(x => x.Goal != null);
            RuleFor(x => x.Goal.ExpectedImprovement)
                .GreaterThan(0)
                .When(x => x.Goal != null)
                .WithMessage("Expected improvement must be greater than 0.");
                
            RuleFor(x => x.Goal.MinimumViableImprovement)
                .GreaterThan(0)
                .LessThanOrEqualTo(x => x.Goal.ExpectedImprovement)
                .When(x => x.Goal != null)
                .WithMessage("Minimum viable improvement must be greater than 0 and no more than the expected improvement.");

            RuleFor(x => x.PrimaryMetric).NotNull();
            RuleFor(x => x.PrimaryMetric.Name)
                .NotEmpty()
                .When(x => x.PrimaryMetric != null)
                .WithMessage("Primary metric name is required.");
        }
    }

    public class CreateExperimentVariantDtoValidator : AbstractValidator<CreateExperimentVariantDto>
    {
        public CreateExperimentVariantDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Variant name is required and must be at most 100 characters.");

            RuleFor(x => x.Setup).NotNull();
            RuleFor(x => x.Setup.TrafficAllocation)
                .InclusiveBetween(0, 1)
                .When(x => x.Setup != null)
                .WithMessage("Traffic allocation must be between 0 and 1.");
                
            RuleFor(x => x.Setup.Configuration)
                .NotEmpty()
                .When(x => x.Setup != null)
                .WithMessage("Variant configuration is required.");
        }
    }
}
