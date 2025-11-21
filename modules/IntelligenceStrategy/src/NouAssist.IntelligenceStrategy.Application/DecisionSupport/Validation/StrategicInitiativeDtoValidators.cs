using FluentValidation;
using NouAssist.IntelligenceStrategy.DecisionSupport.Constants;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;
using NouAssist.IntelligenceStrategy.DecisionSupport.StrategicInitiatives.Dtos;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Validation
{
    /// <summary>
    /// FluentValidation validator for <see cref="CreateStrategicInitiativeDto"/>
    /// </summary>
    public class CreateStrategicInitiativeDtoValidator : AbstractValidator<CreateStrategicInitiativeDto>
    {
        /// <summary>
        /// Creates a new instance of <see cref="CreateStrategicInitiativeDtoValidator"/>
        /// </summary>
        public CreateStrategicInitiativeDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(DecisionSupportConsts.MaxTitleLength);
                
            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(DecisionSupportConsts.MaxDescriptionLength);
                
            RuleFor(x => x.Rationale)
                .NotEmpty()
                .MaximumLength(DecisionSupportConsts.MaxRationaleLength);
                
            RuleFor(x => x.ExpectedOutcomes)
                .NotEmpty()
                .MaximumLength(DecisionSupportConsts.MaxExpectedOutcomesLength);
                
            RuleFor(x => x.StrategicAlignment)
                .IsInEnum();
        }
    }

    /// <summary>
    /// FluentValidation validator for <see cref="UpdateStrategicInitiativeDto"/>
    /// </summary>
    public class UpdateStrategicInitiativeDtoValidator : AbstractValidator<UpdateStrategicInitiativeDto>
    {
        /// <summary>
        /// Creates a new instance of <see cref="UpdateStrategicInitiativeDtoValidator"/>
        /// </summary>
        public UpdateStrategicInitiativeDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(DecisionSupportConsts.MaxTitleLength);
                
            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(DecisionSupportConsts.MaxDescriptionLength);
                
            RuleFor(x => x.Rationale)
                .NotEmpty()
                .MaximumLength(DecisionSupportConsts.MaxRationaleLength);
                
            RuleFor(x => x.ExpectedOutcomes)
                .NotEmpty()
                .MaximumLength(DecisionSupportConsts.MaxExpectedOutcomesLength);
                
            RuleFor(x => x.StrategicAlignment)
                .IsInEnum();
        }
    }

    /// <summary>
    /// FluentValidation validator for <see cref="RejectInitiativeDto"/>
    /// </summary>
    public class RejectInitiativeDtoValidator : AbstractValidator<RejectInitiativeDto>
    {
        /// <summary>
        /// Creates a new instance of <see cref="RejectInitiativeDtoValidator"/>
        /// </summary>
        public RejectInitiativeDtoValidator()
        {
            RuleFor(x => x.RejectionReason)
                .NotEmpty()
                .MaximumLength(DecisionSupportConsts.MaxDescriptionLength);
        }
    }
}
