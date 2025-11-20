using FluentValidation;
using NouAssist.IntelligenceStrategy.DecisionSupport.DecisionModels.Dtos;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Validation
{
    /// <summary>
    /// FluentValidation validator for <see cref="CreateDecisionModelDto"/>
    /// </summary>
    public class CreateDecisionModelDtoValidator : AbstractValidator<CreateDecisionModelDto>
    {
        /// <summary>
        /// Creates a new instance of <see cref="CreateDecisionModelDtoValidator"/>
        /// </summary>
        public CreateDecisionModelDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(DecisionSupportConsts.MaxTitleLength);
                
            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(DecisionSupportConsts.MaxDescriptionLength);
                
            RuleFor(x => x.ModelType)
                .NotEmpty()
                .MaximumLength(50);
        }
    }

    /// <summary>
    /// FluentValidation validator for <see cref="AddCriterionDto"/>
    /// </summary>
    public class AddCriterionDtoValidator : AbstractValidator<AddCriterionDto>
    {
        /// <summary>
        /// Creates a new instance of <see cref="AddCriterionDtoValidator"/>
        /// </summary>
        public AddCriterionDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);
                
            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(DecisionSupportConsts.MaxDescriptionLength);
                
            RuleFor(x => x.Weight)
                .InclusiveBetween(0, 1)
                .WithMessage("Weight must be between 0 and 1");
        }
    }

    /// <summary>
    /// FluentValidation validator for <see cref="AddAlternativeDto"/>
    /// </summary>
    public class AddAlternativeDtoValidator : AbstractValidator<AddAlternativeDto>
    {
        /// <summary>
        /// Creates a new instance of <see cref="AddAlternativeDtoValidator"/>
        /// </summary>
        public AddAlternativeDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);
                
            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(DecisionSupportConsts.MaxDescriptionLength);
        }
    }

    /// <summary>
    /// FluentValidation validator for <see cref="SetScoreDto"/>
    /// </summary>
    public class SetScoreDtoValidator : AbstractValidator<SetScoreDto>
    {
        /// <summary>
        /// Creates a new instance of <see cref="SetScoreDtoValidator"/>
        /// </summary>
        public SetScoreDtoValidator()
        {
            RuleFor(x => x.CriterionId)
                .NotEmpty();
                
            RuleFor(x => x.Score)
                .InclusiveBetween(0, 1)
                .WithMessage("Score must be between 0 and 1");
        }
    }
}
