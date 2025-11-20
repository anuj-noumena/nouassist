using FluentValidation;
using NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Tasks.Validators
{
    public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
    {
        public CreateTaskDtoValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("Project ID is required");
                
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Task name is required")
                .MaximumLength(ProjectExecutionConsts.MaxTaskNameLength)
                .WithMessage($"Task name cannot exceed {ProjectExecutionConsts.MaxTaskNameLength} characters");
                
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Task description is required")
                .MaximumLength(ProjectExecutionConsts.MaxTaskDescriptionLength)
                .WithMessage($"Task description cannot exceed {ProjectExecutionConsts.MaxTaskDescriptionLength} characters");
                
            RuleFor(x => x.EstimatedHours)
                .GreaterThan(0)
                .WithMessage("Estimated hours must be greater than zero");
                
            RuleFor(x => x.DueDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .When(x => x.StartDate.HasValue && x.DueDate.HasValue)
                .WithMessage("Due date must be on or after start date");
        }
    }
}
