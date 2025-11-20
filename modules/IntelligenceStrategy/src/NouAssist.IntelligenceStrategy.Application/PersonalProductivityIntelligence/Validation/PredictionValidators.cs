using System;
using FluentValidation;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Constants;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Predictions.Dtos;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Validation
{
    /// <summary>
    /// Validator for PredictTaskDurationDto
    /// </summary>
    public class PredictTaskDurationDtoValidator : AbstractValidator<PredictTaskDurationDto>
    {
        public PredictTaskDurationDtoValidator()
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
                
            RuleFor(x => x.TaskDescription)
                .MaximumLength(PersonalProductivityIntelligenceConsts.MaxDescriptionLength)
                .WithMessage($"Task description must not exceed {PersonalProductivityIntelligenceConsts.MaxDescriptionLength} characters");
                
            RuleFor(x => x.TaskContext)
                .SetValidator(new UserProfiles.Dtos.TaskContextDtoValidator())
                .When(x => x.TaskContext != null);
        }
    }
    
    /// <summary>
    /// Validator for PredictRiskDto
    /// </summary>
    public class PredictRiskDtoValidator : AbstractValidator<PredictRiskDto>
    {
        public PredictRiskDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required");
                
            RuleFor(x => x.TargetEntityType)
                .NotEmpty()
                .WithMessage("Target entity type is required")
                .MaximumLength(50)
                .WithMessage("Target entity type must not exceed 50 characters");
                
            RuleFor(x => x.TargetEntityId)
                .NotEmpty()
                .WithMessage("Target entity ID is required");
                
            RuleFor(x => x.EntityName)
                .NotEmpty()
                .WithMessage("Entity name is required")
                .MaximumLength(PersonalProductivityIntelligenceConsts.MaxNameLength)
                .WithMessage($"Entity name must not exceed {PersonalProductivityIntelligenceConsts.MaxNameLength} characters");
                
            RuleFor(x => x.EntityDescription)
                .MaximumLength(PersonalProductivityIntelligenceConsts.MaxDescriptionLength)
                .WithMessage($"Entity description must not exceed {PersonalProductivityIntelligenceConsts.MaxDescriptionLength} characters");
        }
    }
    
    /// <summary>
    /// Validator for RecordActualOutcomeDto
    /// </summary>
    public class RecordActualOutcomeDtoValidator : AbstractValidator<RecordActualOutcomeDto>
    {
        public RecordActualOutcomeDtoValidator()
        {
            RuleFor(x => x.PredictionId)
                .NotEmpty()
                .WithMessage("Prediction ID is required");
        }
    }
}
