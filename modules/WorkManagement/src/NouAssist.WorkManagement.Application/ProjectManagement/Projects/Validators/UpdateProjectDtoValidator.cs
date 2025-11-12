using FluentValidation;
using NouAssist.WorkManagement.ProjectManagement.Constants;
using NouAssist.WorkManagement.ProjectManagement.Projects.Dtos;

namespace NouAssist.WorkManagement.ProjectManagement.Projects.Validators
{
    public class UpdateProjectDtoValidator : AbstractValidator<UpdateProjectDto>
    {
        public UpdateProjectDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(ProjectDomainConstants.MaxTitleLength);

            RuleFor(x => x.Description)
                .MaximumLength(ProjectDomainConstants.MaxDescriptionLength)
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleFor(x => x.Summary)
                .MaximumLength(ProjectDomainConstants.MaxSummaryLength)
                .When(x => !string.IsNullOrWhiteSpace(x.Summary));

            RuleFor(x => x.ExpectedOutcome)
                .MaximumLength(ProjectDomainConstants.MaxDescriptionLength)
                .When(x => !string.IsNullOrWhiteSpace(x.ExpectedOutcome));

            RuleFor(x => x.Importance)
                .InclusiveBetween(0.0, 1.0);

            RuleFor(x => x.Urgency)
                .InclusiveBetween(0.0, 1.0);

            RuleFor(x => x.Progress)
                .InclusiveBetween(0, 100);

            RuleFor(x => x.DueDate)
                .GreaterThan(x => x.StartDate)
                .When(x => x.StartDate.HasValue && x.DueDate.HasValue)
                .WithMessage("Due date must be after start date.");
        }
    }
}
