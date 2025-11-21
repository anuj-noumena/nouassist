using System;
using FluentValidation;
using NouAssist.BusinessOperations.ProjectExecution.Constants;
using NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Validators
{
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
                .WithMessage("Hours cannot exceed 24 per time log entry");
                
            RuleFor(x => x.LogDate)
                .NotEmpty()
                .WithMessage("Log date is required")
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Log date cannot be in the future");
                
            RuleFor(x => x.Notes)
                .MaximumLength(ProjectExecutionConsts.MaxTimeLogNotesLength)
                .When(x => x.Notes != null)
                .WithMessage($"Notes cannot exceed {ProjectExecutionConsts.MaxTimeLogNotesLength} characters");
        }
    }
}
