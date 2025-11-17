using FluentValidation;
using NouAssist.LearningDevelopment.SkillDevelopment.Skills.Dtos;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Skills
{
    public class UpdateSkillProficiencyDtoValidator : AbstractValidator<UpdateSkillProficiencyDto>
    {
        public UpdateSkillProficiencyDtoValidator()
        {
            RuleFor(x => x.Score)
                .InclusiveBetween(0, 100)
                .WithMessage("Proficiency score must be between 0 and 100");

            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .WithMessage("Assessment notes cannot exceed 1000 characters");
        }
    }
}
