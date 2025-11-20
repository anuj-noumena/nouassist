using FluentValidation;
using NouAssist.RequirementsManagement.QualityAssurance.TestPlans.Dtos;

namespace NouAssist.RequirementsManagement.QualityAssurance.Validation
{
    public class CreateTestPlanDtoValidator : AbstractValidator<CreateTestPlanDto>
    {
        public CreateTestPlanDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Test plan name is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Test plan description is required");
            RuleFor(x => x.ReleaseVersion).NotEmpty().WithMessage("Release version is required");
            RuleFor(x => x.StrategyApproach).NotEmpty().WithMessage("Testing approach is required");
            RuleFor(x => x.StrategyCoverageDescription).NotEmpty().WithMessage("Coverage description is required");
            RuleFor(x => x.EnvironmentName).NotEmpty().WithMessage("Environment name is required");
            RuleFor(x => x.EnvironmentDescription).NotEmpty().WithMessage("Environment description is required");
            
            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after start date");
        }
    }

    public class UpdateTestPlanDtoValidator : AbstractValidator<UpdateTestPlanDto>
    {
        public UpdateTestPlanDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Test plan name is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Test plan description is required");
            RuleFor(x => x.ReleaseVersion).NotEmpty().WithMessage("Release version is required");
            RuleFor(x => x.StrategyApproach).NotEmpty().WithMessage("Testing approach is required");
            RuleFor(x => x.StrategyCoverageDescription).NotEmpty().WithMessage("Coverage description is required");
            RuleFor(x => x.EnvironmentName).NotEmpty().WithMessage("Environment name is required");
            RuleFor(x => x.EnvironmentDescription).NotEmpty().WithMessage("Environment description is required");
            
            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after start date");
        }
    }
}
