using FluentValidation;
using NouAssist.BusinessOperations.ProjectExecution.Constants;
using NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Validators
{
    public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
    {
        public CreateProjectDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Project name is required")
                .MaximumLength(ProjectExecutionConsts.MaxProjectNameLength)
                .WithMessage($"Project name cannot exceed {ProjectExecutionConsts.MaxProjectNameLength} characters");
                
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Project description is required")
                .MaximumLength(ProjectExecutionConsts.MaxProjectDescriptionLength)
                .WithMessage($"Project description cannot exceed {ProjectExecutionConsts.MaxProjectDescriptionLength} characters");
                
            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Start date is required");
                
            RuleFor(x => x.PlannedEndDate)
                .NotEmpty()
                .WithMessage("Planned end date is required")
                .GreaterThan(x => x.StartDate)
                .WithMessage("Planned end date must be after start date");
        }
    }
}
