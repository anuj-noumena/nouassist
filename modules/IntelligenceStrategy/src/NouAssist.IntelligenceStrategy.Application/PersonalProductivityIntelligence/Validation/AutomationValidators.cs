using System;
using FluentValidation;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Automations.Dtos;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Constants;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Validation
{
    /// <summary>
    /// Validator for CreateAutomationDto
    /// </summary>
    public class CreateAutomationDtoValidator : AbstractValidator<CreateAutomationDto>
    {
        public CreateAutomationDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required");
                
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(PersonalProductivityIntelligenceConsts.MaxNameLength)
                .WithMessage($"Name must not exceed {PersonalProductivityIntelligenceConsts.MaxNameLength} characters");
                
            RuleFor(x => x.Description)
                .MaximumLength(PersonalProductivityIntelligenceConsts.MaxDescriptionLength)
                .WithMessage($"Description must not exceed {PersonalProductivityIntelligenceConsts.MaxDescriptionLength} characters");
        }
    }
    
    /// <summary>
    /// Validator for UpdateAutomationDto
    /// </summary>
    public class UpdateAutomationDtoValidator : AbstractValidator<UpdateAutomationDto>
    {
        public UpdateAutomationDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(PersonalProductivityIntelligenceConsts.MaxNameLength)
                .WithMessage($"Name must not exceed {PersonalProductivityIntelligenceConsts.MaxNameLength} characters");
                
            RuleFor(x => x.Description)
                .MaximumLength(PersonalProductivityIntelligenceConsts.MaxDescriptionLength)
                .WithMessage($"Description must not exceed {PersonalProductivityIntelligenceConsts.MaxDescriptionLength} characters");
        }
    }
    
    /// <summary>
    /// Validator for AddTriggerDto
    /// </summary>
    public class AddTriggerDtoValidator : AbstractValidator<AddTriggerDto>
    {
        public AddTriggerDtoValidator()
        {
            RuleFor(x => x.AutomationId)
                .NotEmpty()
                .WithMessage("Automation ID is required");
                
            RuleFor(x => x.TriggerType)
                .NotEmpty()
                .WithMessage("Trigger type is required")
                .MaximumLength(50)
                .WithMessage("Trigger type must not exceed 50 characters")
                .Must(BeValidTriggerType)
                .WithMessage("Invalid trigger type. Must be one of: TimeBased, EventBased, ConditionBased, Manual, LocationBased");
                
            RuleFor(x => x.TriggerData)
                .NotEmpty()
                .WithMessage("Trigger data is required");
                
            RuleFor(x => x.TriggerDescription)
                .MaximumLength(256)
                .WithMessage("Trigger description must not exceed 256 characters");
        }
        
        private bool BeValidTriggerType(string triggerType)
        {
            return Enum.TryParse<AutomationTriggerType>(triggerType, out _);
        }
    }
    
    /// <summary>
    /// Validator for AddActionDto
    /// </summary>
    public class AddActionDtoValidator : AbstractValidator<AddActionDto>
    {
        public AddActionDtoValidator()
        {
            RuleFor(x => x.AutomationId)
                .NotEmpty()
                .WithMessage("Automation ID is required");
                
            RuleFor(x => x.ActionType)
                .NotEmpty()
                .WithMessage("Action type is required")
                .MaximumLength(50)
                .WithMessage("Action type must not exceed 50 characters");
                
            RuleFor(x => x.ActionData)
                .NotEmpty()
                .WithMessage("Action data is required");
                
            RuleFor(x => x.ActionDescription)
                .MaximumLength(256)
                .WithMessage("Action description must not exceed 256 characters");
        }
    }
}
