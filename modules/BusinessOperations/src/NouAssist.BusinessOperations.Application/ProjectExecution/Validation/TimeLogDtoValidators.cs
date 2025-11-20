using FluentValidation;
using NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Validation
{
    /// <summary>
    /// Validator for CreateTimeLogDto
    /// </summary>
    public class CreateTimeLogDtoValidator : AbstractValidator<CreateTimeLogDto>
    {
        public CreateTimeLogDtoValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("Project ID is required");
                
            RuleFor(x => x.Hours)
                .GreaterThan(0)
                .WithMessage("Hours must be greater than zero")
                .LessThanOrEqualTo(24)
                .WithMessage("Hours cannot exceed 24 per entry");
                
            RuleFor(x => x.LogDate)
                .NotEmpty()
                .WithMessage("Log date is required");
                
            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .WithMessage("Notes cannot exceed 1000 characters");
        }
    }
    
    /// <summary>
    /// Validator for UpdateTimeLogDto
    /// </summary>
    public class UpdateTimeLogDtoValidator : AbstractValidator<UpdateTimeLogDto>
    {
        public UpdateTimeLogDtoValidator()
        {
            RuleFor(x => x.Hours)
                .GreaterThan(0)
                .WithMessage("Hours must be greater than zero")
                .LessThanOrEqualTo(24)
                .WithMessage("Hours cannot exceed 24 per entry");
                
            RuleFor(x => x.LogDate)
                .NotEmpty()
                .WithMessage("Log date is required");
                
            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .WithMessage("Notes cannot exceed 1000 characters");
        }
    }
}
