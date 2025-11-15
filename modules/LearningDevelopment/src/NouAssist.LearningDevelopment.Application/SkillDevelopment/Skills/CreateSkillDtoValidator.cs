using FluentValidation;
using NouAssist.SkillDevelopment.Skills.Dtos;

namespace NouAssist.SkillDevelopment.Skills
{
    public class CreateSkillDtoValidator : AbstractValidator<CreateSkillDto>
    {
        public CreateSkillDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Skill title is required")
                .MaximumLength(SkillConsts.MaxTitleLength);

            RuleFor(x => x.Description)
                .MaximumLength(SkillConsts.MaxDescriptionLength);

            RuleFor(x => x.EstimatedPracticeHours)
                .GreaterThan(0).WithMessage("Estimated practice hours must be greater than 0")
                .LessThanOrEqualTo(10000).WithMessage("Estimated practice hours cannot exceed 10000");

            RuleFor(x => x.Category)
                .MaximumLength(SkillConsts.MaxCategoryLength);

            RuleFor(x => x.DevelopmentGoals)
                .MaximumLength(SkillConsts.MaxDevelopmentGoalsLength);

            RuleFor(x => x.Prerequisites)
                .Must(list => list == null || list.Count <= 20)
                .WithMessage("A skill cannot have more than 20 prerequisites");

            RuleFor(x => x.Resources)
                .Must(list => list == null || list.Count <= 50)
                .WithMessage("A skill cannot have more than 50 resources");
        }
    }
}
