using FluentValidation;
using NouAssist.LearningDevelopment.SkillDevelopment.SkillLogs.Dtos;
using NouAssist.SkillDevelopment;

namespace NouAssist.LearningDevelopment.SkillDevelopment.SkillLogs
{
    public class CreateSkillLogDtoValidator : AbstractValidator<CreateSkillLogDto>
    {
        public CreateSkillLogDtoValidator()
        {
            RuleFor(x => x.SkillId)
                .NotEmpty().WithMessage("Skill ID is required");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Log content is required")
                .MaximumLength(SkillLogConsts.MaxContentLength);

            RuleFor(x => x.ProficiencyScore)
                .InclusiveBetween(0, 100)
                .When(x => x.ProficiencyScore.HasValue)
                .WithMessage("Proficiency score must be between 0 and 100");
        }
    }
}
