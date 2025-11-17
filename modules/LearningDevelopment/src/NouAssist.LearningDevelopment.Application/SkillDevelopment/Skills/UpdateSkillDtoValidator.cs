using FluentValidation;
using NouAssist.LearningDevelopment.SkillDevelopment.Skills.Dtos;
using NouAssist.SkillDevelopment;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Skills
{
    public class UpdateSkillDtoValidator : AbstractValidator<UpdateSkillDto>
    {
        public UpdateSkillDtoValidator()
        {
            RuleFor(x => x.Description)
                .MaximumLength(SkillConsts.MaxDescriptionLength);

            RuleFor(x => x.Category)
                .MaximumLength(SkillConsts.MaxCategoryLength);

            RuleFor(x => x.DevelopmentGoals)
                .MaximumLength(SkillConsts.MaxDevelopmentGoalsLength);

            RuleFor(x => x.Resources)
                .Must(list => list == null || list.Count <= 50)
                .WithMessage("A skill cannot have more than 50 resources");
        }
    }
}
