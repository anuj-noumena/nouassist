using FluentValidation;
using NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Validation
{
    /// <summary>
    /// Validator for CreateProjectDto
    /// </summary>
    public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
    {
        public CreateProjectDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Project name is required")
                .MaximumLength(100)
                .WithMessage("Project name cannot exceed 100 characters");
                
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Project description is required")
                .MaximumLength(2000)
                .WithMessage("Project description cannot exceed 2000 characters");
                
            RuleFor(x => x.PlannedEndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("Planned end date must be after start date");
        }
    }
    
    /// <summary>
    /// Validator for UpdateProjectDto
    /// </summary>
    public class UpdateProjectDtoValidator : AbstractValidator<UpdateProjectDto>
    {
        public UpdateProjectDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Project name is required")
                .MaximumLength(100)
                .WithMessage("Project name cannot exceed 100 characters");
                
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Project description is required")
                .MaximumLength(2000)
                .WithMessage("Project description cannot exceed 2000 characters");
                
            RuleFor(x => x.PlannedEndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("Planned end date must be after start date");
        }
    }
    
    /// <summary>
    /// Validator for CreateProjectPhaseDto
    /// </summary>
    public class CreateProjectPhaseDtoValidator : AbstractValidator<CreateProjectPhaseDto>
    {
        public CreateProjectPhaseDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Phase name is required")
                .MaximumLength(100)
                .WithMessage("Phase name cannot exceed 100 characters");
                
            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after start date");
                
            RuleFor(x => x.Order)
                .GreaterThan(0)
                .WithMessage("Order must be greater than zero");
        }
    }
    
    /// <summary>
    /// Validator for CreateProjectMilestoneDto
    /// </summary>
    public class CreateProjectMilestoneDtoValidator : AbstractValidator<CreateProjectMilestoneDto>
    {
        public CreateProjectMilestoneDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Milestone name is required")
                .MaximumLength(100)
                .WithMessage("Milestone name cannot exceed 100 characters");
                
            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Milestone description cannot exceed 1000 characters");
        }
    }
}
