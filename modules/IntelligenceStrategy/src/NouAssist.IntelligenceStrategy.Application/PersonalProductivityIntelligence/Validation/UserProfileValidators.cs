using System;
using FluentValidation;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Constants;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.UserProfiles.Dtos;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Validation
{
    /// <summary>
    /// Validator for TaskContextDto
    /// </summary>
    public class TaskContextDtoValidator : AbstractValidator<TaskContextDto>
    {
        public TaskContextDtoValidator()
        {
            RuleFor(x => x.Tags)
                .NotNull()
                .WithMessage("Tags collection cannot be null");
                
            RuleFor(x => x.Location)
                .MaximumLength(256)
                .WithMessage("Location must not exceed 256 characters");
                
            RuleFor(x => x.EnergyLevel)
                .Must(x => string.IsNullOrEmpty(x) || 
                           x == "High" || 
                           x == "Medium" || 
                           x == "Low")
                .WithMessage("Energy level must be High, Medium, or Low");
                
            RuleFor(x => x.FocusLevel)
                .Must(x => string.IsNullOrEmpty(x) || 
                           x == "High" || 
                           x == "Medium" || 
                           x == "Low")
                .WithMessage("Focus level must be High, Medium, or Low");
        }
    }
    
    /// <summary>
    /// Validator for RecordTaskCompletionDto
    /// </summary>
    public class RecordTaskCompletionDtoValidator : AbstractValidator<RecordTaskCompletionDto>
    {
        public RecordTaskCompletionDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required");
                
            RuleFor(x => x.TaskId)
                .NotEmpty()
                .WithMessage("Task ID is required");
                
            RuleFor(x => x.TaskName)
                .NotEmpty()
                .WithMessage("Task name is required")
                .MaximumLength(PersonalProductivityIntelligenceConsts.MaxNameLength)
                .WithMessage($"Task name must not exceed {PersonalProductivityIntelligenceConsts.MaxNameLength} characters");
                
            RuleFor(x => x.EstimatedDuration)
                .GreaterThan(0)
                .WithMessage("Estimated duration must be greater than 0");
                
            RuleFor(x => x.ActualDuration)
                .GreaterThan(0)
                .WithMessage("Actual duration must be greater than 0");
                
            RuleFor(x => x.TaskContext)
                .SetValidator(new TaskContextDtoValidator())
                .When(x => x.TaskContext != null);
        }
    }
    
    /// <summary>
    /// Validator for UpdateUserContextDto
    /// </summary>
    public class UpdateUserContextDtoValidator : AbstractValidator<UpdateUserContextDto>
    {
        public UpdateUserContextDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required");
                
            RuleFor(x => x.EntityType)
                .NotEmpty()
                .WithMessage("Entity type is required")
                .MaximumLength(128)
                .WithMessage("Entity type must not exceed 128 characters");
                
            RuleFor(x => x.EntityId)
                .NotEmpty()
                .WithMessage("Entity ID is required");
        }
    }
}
