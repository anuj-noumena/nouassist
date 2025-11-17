using FluentValidation;
using NouAssist.LearningDevelopment.Learning.Topics.Dtos;

namespace NouAssist.LearningDevelopment.Learning.Topics.Validators
{
    public class UpdateTopicDtoValidator : AbstractValidator<UpdateTopicDto>
    {
        public UpdateTopicDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.Description)
                .MaximumLength(2000)
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.EstimatedHours)
                .GreaterThan(0)
                .LessThanOrEqualTo(1000);
        }
    }
}
