using FluentValidation;
using NouAssist.CoreProductivity.ProjectManagement.Constants;
using NouAssist.CoreProductivity.ProjectManagement.Sprints.Dtos;

namespace NouAssist.CoreProductivity.ProjectManagement.Sprints.Validators
{
    public class CreateSprintDtoValidator : AbstractValidator<CreateSprintDto>
    {
        public CreateSprintDtoValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty();

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

            RuleFor(x => x.VelocityEstimate)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(ProjectDomainConstants.MaxStoryPoints);
        }
    }
}
