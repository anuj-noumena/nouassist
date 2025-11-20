using FluentValidation;
using NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Validation
{
    /// <summary>
    /// Validator for CreateTaskDto
    /// </summary>
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
                .MaximumLength(200)
                .WithMessage("Task name cannot exceed 200 characters");
                
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Task description is required")
                .MaximumLength(2000)
                .WithMessage("Task description cannot exceed 2000 characters");
                
            RuleFor(x => x.EstimatedHours)
                .GreaterThan(0)
                .WithMessage("Estimated hours must be greater than zero");
                
            RuleFor(x => x.DueDate)
                .GreaterThan(x => x.StartDate)
                .When(x => x.StartDate.HasValue && x.DueDate.HasValue)
                .WithMessage("Due date must be after start date");
        }
    }
    
    /// <summary>
    /// Validator for UpdateTaskDto
    /// </summary>
    public class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Task name is required")
                .MaximumLength(200)
                .WithMessage("Task name cannot exceed 200 characters");
                
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Task description is required")
                .MaximumLength(2000)
                .WithMessage("Task description cannot exceed 2000 characters");
                
            RuleFor(x => x.EstimatedHours)
                .GreaterThan(0)
                .WithMessage("Estimated hours must be greater than zero");
                
            RuleFor(x => x.DueDate)
                .GreaterThan(x => x.StartDate)
                .When(x => x.StartDate.HasValue && x.DueDate.HasValue)
                .WithMessage("Due date must be after start date");
        }
    }
    
    /// <summary>
    /// Validator for AddTaskDependencyDto
    /// </summary>
    public class AddTaskDependencyDtoValidator : AbstractValidator<AddTaskDependencyDto>
    {
        public AddTaskDependencyDtoValidator()
        {
            RuleFor(x => x.DependsOnTaskId)
                .NotEmpty()
                .WithMessage("Dependency task ID is required");
        }
    }
}
