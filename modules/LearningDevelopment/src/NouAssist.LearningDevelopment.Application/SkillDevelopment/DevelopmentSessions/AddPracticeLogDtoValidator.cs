using FluentValidation;
using NouAssist.SkillDevelopment.DevelopmentSessions.Dtos;

namespace NouAssist.SkillDevelopment.DevelopmentSessions
{
    public class AddPracticeLogDtoValidator : AbstractValidator<AddPracticeLogDto>
    {
        public AddPracticeLogDtoValidator()
        {
            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0).WithMessage("Practice duration must be greater than 0")
                .LessThanOrEqualTo(1440).WithMessage("Practice duration cannot exceed 24 hours");

            RuleFor(x => x.SelfRating)
                .InclusiveBetween(1, 10)
                .When(x => x.SelfRating.HasValue)
                .WithMessage("Self rating must be between 1 and 10");

            RuleFor(x => x.FocusArea)
                .MaximumLength(PracticeLogConsts.MaxFocusAreaLength);

            RuleFor(x => x.Notes)
                .MaximumLength(PracticeLogConsts.MaxNotesLength);
        }
    }
}
