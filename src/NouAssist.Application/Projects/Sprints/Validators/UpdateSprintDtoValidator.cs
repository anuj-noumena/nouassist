using FluentValidation;
using NouAssist.Projects.Constants;
using NouAssist.Projects.Sprints.Dtos;

namespace NouAssist.Projects.Sprints.Validators
{
    public class UpdateSprintDtoValidator : AbstractValidator<UpdateSprintDto>
    {
        public UpdateSprintDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Goal)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(x => x.ReleaseTheme)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.ReleaseTheme));

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after start date.");

            RuleFor(x => x)
                .Must(x => (x.EndDate - x.StartDate).Days >= ProjectDomainConstants.MinSprintDurationDays)
                .WithMessage($"Sprint duration must be at least {ProjectDomainConstants.MinSprintDurationDays} days.");

            RuleFor(x => x)
                .Must(x => (x.EndDate - x.StartDate).Days <= ProjectDomainConstants.MaxSprintDurationDays)
                .WithMessage($"Sprint duration cannot exceed {ProjectDomainConstants.MaxSprintDurationDays} days.");
        }
    }
}
