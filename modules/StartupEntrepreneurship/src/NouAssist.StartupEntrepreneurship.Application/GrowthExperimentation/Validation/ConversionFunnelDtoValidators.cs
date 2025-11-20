using FluentValidation;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ConversionFunnels.Dtos;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Validation
{
    public class CreateFunnelStageDtoValidator : AbstractValidator<CreateFunnelStageDto>
    {
        public CreateFunnelStageDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(GrowthExperimentationConstants.ConversionFunnels.MaxStageNameLength)
                .WithMessage($"Stage name is required and must be at most {GrowthExperimentationConstants.ConversionFunnels.MaxStageNameLength} characters.");

            RuleFor(x => x.Order)
                .GreaterThan(0)
                .LessThanOrEqualTo(GrowthExperimentationConstants.ConversionFunnels.MaxFunnelStages)
                .WithMessage($"Order must be between 1 and {GrowthExperimentationConstants.ConversionFunnels.MaxFunnelStages}.");
        }
    }

    public class UpdateStageConversionRateDtoValidator : AbstractValidator<UpdateStageConversionRateDto>
    {
        public UpdateStageConversionRateDtoValidator()
        {
            RuleFor(x => x.EnteredCount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Entered count must be a non-negative value.");

            RuleFor(x => x.CompletedCount)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(x => x.EnteredCount)
                .WithMessage("Completed count must be between 0 and entered count.");
        }
    }
}
