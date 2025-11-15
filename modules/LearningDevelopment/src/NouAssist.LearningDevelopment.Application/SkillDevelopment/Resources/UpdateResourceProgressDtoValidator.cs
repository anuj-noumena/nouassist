using FluentValidation;
using NouAssist.SkillDevelopment.Resources.Dtos;

namespace NouAssist.SkillDevelopment.Resources
{
    public class UpdateResourceProgressDtoValidator : AbstractValidator<UpdateResourceProgressDto>
    {
        public UpdateResourceProgressDtoValidator()
        {
            RuleFor(x => x.Progress)
                .InclusiveBetween(0, 100)
                .WithMessage("Progress must be between 0 and 100");
        }
    }
}
