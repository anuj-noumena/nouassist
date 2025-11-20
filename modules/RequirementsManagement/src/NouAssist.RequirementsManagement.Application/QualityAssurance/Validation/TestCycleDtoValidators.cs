using FluentValidation;
using NouAssist.RequirementsManagement.QualityAssurance.TestCycles.Dtos;

namespace NouAssist.RequirementsManagement.QualityAssurance.Validation
{
    public class CreateTestCycleDtoValidator : AbstractValidator<CreateTestCycleDto>
    {
        public CreateTestCycleDtoValidator()
        {
            RuleFor(x => x.TestPlanId).NotEmpty().WithMessage("Test plan ID is required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Test cycle name is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Test cycle description is required");
            RuleFor(x => x.BuildInformation).NotEmpty().WithMessage("Build information is required");
            RuleFor(x => x.AssigneeId).NotEmpty().WithMessage("Assignee is required");
            
            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after start date");
        }
    }

    public class UpdateTestCycleDtoValidator : AbstractValidator<UpdateTestCycleDto>
    {
        public UpdateTestCycleDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Test cycle name is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Test cycle description is required");
            RuleFor(x => x.BuildInformation).NotEmpty().WithMessage("Build information is required");
            RuleFor(x => x.AssigneeId).NotEmpty().WithMessage("Assignee is required");
            
            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after start date");
        }
    }
}
