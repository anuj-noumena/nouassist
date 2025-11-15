using FluentValidation;
using NouAssist.SkillDevelopment.SkillLogs.Dtos;

namespace NouAssist.SkillDevelopment.SkillLogs
{
    public class UpdateSkillLogDtoValidator : AbstractValidator<UpdateSkillLogDto>
    {
        public UpdateSkillLogDtoValidator()
        {
            RuleFor(x => x.Content)
                .MaximumLength(SkillLogConsts.MaxContentLength)
                .When(x => !string.IsNullOrWhiteSpace(x.Content));

            RuleFor(x => x.ProficiencyScore)
                .InclusiveBetween(0, 100)
                .When(x => x.ProficiencyScore.HasValue)
                .WithMessage("Proficiency score must be between 0 and 100");

            RuleFor(x => x.Tags)
                .Must(tags => tags == null || tags.Count <= 20)
                .WithMessage("Cannot have more than 20 tags");
        }
    }
}
