using FluentValidation;
using NouAssist.Learning.Topics.Dtos;

namespace NouAssist.Learning.Topics.Validators
{
    public class CreateTopicDtoValidator : AbstractValidator<CreateTopicDto>
    {
        public CreateTopicDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.Description)
                .MaximumLength(2000)
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Category)
                .MaximumLength(128)
                .When(x => !string.IsNullOrEmpty(x.Category));

            RuleFor(x => x.EstimatedHours)
                .GreaterThan(0)
                .LessThanOrEqualTo(1000);

            RuleFor(x => x.LearningGoals)
                .MaximumLength(2000)
                .When(x => !string.IsNullOrEmpty(x.LearningGoals));
        }
    }
}
