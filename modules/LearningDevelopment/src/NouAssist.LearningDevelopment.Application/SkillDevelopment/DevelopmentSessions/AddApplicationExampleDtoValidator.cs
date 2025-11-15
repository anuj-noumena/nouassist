using FluentValidation;
using NouAssist.SkillDevelopment.DevelopmentSessions.Dtos;

namespace NouAssist.SkillDevelopment.DevelopmentSessions
{
    public class AddApplicationExampleDtoValidator : AbstractValidator<AddApplicationExampleDto>
    {
        public AddApplicationExampleDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Application description is required")
                .MaximumLength(ApplicationExampleConsts.MaxDescriptionLength);

            RuleFor(x => x.ProficiencyImpact)
                .InclusiveBetween(0, 100)
                .When(x => x.ProficiencyImpact.HasValue)
                .WithMessage("Proficiency impact must be between 0 and 100");

            RuleFor(x => x.CompletedAt)
                .NotEmpty()
                .WithMessage("Completion date is required")
                .LessThanOrEqualTo(System.DateTime.UtcNow)
                .WithMessage("Completion date cannot be in the future");

            RuleFor(x => x.Outcomes)
                .Must(outcomes => outcomes == null || outcomes.Count <= 10)
                .WithMessage("Cannot have more than 10 outcomes");
        }
    }
}
