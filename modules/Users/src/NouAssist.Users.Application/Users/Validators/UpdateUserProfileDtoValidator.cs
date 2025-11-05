using FluentValidation;
using NouAssist.Users.Dtos;

namespace NouAssist.Users.Validators
{
    public class UpdateUserProfileDtoValidator : AbstractValidator<UpdateUserProfileDto>
    {
        public UpdateUserProfileDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.LastName));

            RuleFor(x => x.Bio)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.Bio));
        }
    }

    public class UpdatePomodoroSettingsDtoValidator : AbstractValidator<UpdatePomodoroSettingsDto>
    {
        public UpdatePomodoroSettingsDtoValidator()
        {
            RuleFor(x => x.DefaultPomodoroMinutes)
                .InclusiveBetween(1, 60)
                .When(x => x.DefaultPomodoroMinutes.HasValue)
                .WithMessage("Pomodoro time must be between 1 and 60 minutes.");

            RuleFor(x => x.DefaultShortBreakMinutes)
                .InclusiveBetween(1, 30)
                .When(x => x.DefaultShortBreakMinutes.HasValue)
                .WithMessage("Short break must be between 1 and 30 minutes.");

            RuleFor(x => x.DefaultLongBreakMinutes)
                .InclusiveBetween(1, 60)
                .When(x => x.DefaultLongBreakMinutes.HasValue)
                .WithMessage("Long break must be between 1 and 60 minutes.");
        }
    }

    public class UpdateEisenhowerPreferenceDtoValidator : AbstractValidator<UpdateEisenhowerPreferenceDto>
    {
        public UpdateEisenhowerPreferenceDtoValidator()
        {
            RuleFor(x => x.ImportanceThreshold)
                .InclusiveBetween(0.0, 1.0)
                .When(x => x.ImportanceThreshold.HasValue)
                .WithMessage("Importance threshold must be between 0 and 1.");

            RuleFor(x => x.UrgencyThreshold)
                .InclusiveBetween(0.0, 1.0)
                .When(x => x.UrgencyThreshold.HasValue)
                .WithMessage("Urgency threshold must be between 0 and 1.");
        }
    }
}
