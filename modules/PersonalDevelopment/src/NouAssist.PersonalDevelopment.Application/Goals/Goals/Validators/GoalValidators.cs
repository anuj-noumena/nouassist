using NouAssist.PersonalDevelopment.Goals.Goals.Dtos;

namespace NouAssist.PersonalDevelopment.Goals.Goals.Validators
{
    public class CreateGoalDtoValidator : AbstractValidator<CreateGoalDto>
    {
        public CreateGoalDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Description)
                .MaximumLength(5000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleFor(x => x.ImportanceScore)
                .InclusiveBetween(0.0, 1.0)
                .When(x => x.ImportanceScore.HasValue);

            RuleFor(x => x.UrgencyScore)
                .InclusiveBetween(0.0, 1.0)
                .When(x => x.UrgencyScore.HasValue);

            RuleFor(x => x.SmartDescription)
                .MaximumLength(2000)
                .When(x => !string.IsNullOrWhiteSpace(x.SmartDescription));
        }
    }

    public class UpdateGoalProgressDtoValidator : AbstractValidator<UpdateGoalProgressDto>
    {
        public UpdateGoalProgressDtoValidator()
        {
            RuleFor(x => x.Progress)
                .InclusiveBetween(0, 100)
                .WithMessage("Progress must be between 0 and 100.");
        }
    }

    public class AddGoalMilestoneDtoValidator : AbstractValidator<AddGoalMilestoneDto>
    {
        public AddGoalMilestoneDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Description)
                .MaximumLength(2000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }
}
