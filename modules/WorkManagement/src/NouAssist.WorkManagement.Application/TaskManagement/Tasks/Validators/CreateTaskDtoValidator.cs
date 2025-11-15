using FluentValidation;
using NouAssist.WorkManagement.TaskManagement.Tasks.Dtos;

namespace NouAssist.WorkManagement.TaskManagement.Tasks.Validators
{
    public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
    {
        public CreateTaskDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.Description)
                .MaximumLength(4000);

            RuleFor(x => x.DefaultImportance)
                .InclusiveBetween(0.0, 1.0);

            RuleFor(x => x.DefaultUrgency)
                .InclusiveBetween(0.0, 1.0);

            RuleFor(x => x.DefaultEstimateMinutes)
                .GreaterThan(0)
                .When(x => x.DefaultEstimateMinutes.HasValue);

            // GTD validation: cannot be in both project and area
            RuleFor(x => x)
                .Must(x => !(x.ProjectId.HasValue && x.AreaId.HasValue))
                .WithMessage("Task cannot belong to both a Project and an Area.");

            RuleFor(x => x.ChecklistItems)
                .Must(items => items == null || items.Count <= 100)
                .WithMessage("Maximum 100 checklist items allowed.");

            RuleForEach(x => x.ChecklistItems)
                .SetValidator(new CreateChecklistItemDtoValidator())
                .When(x => x.ChecklistItems != null);

            RuleFor(x => x.AcceptanceCriteria)
                .Must(criteria => criteria == null || criteria.Count <= 20)
                .WithMessage("Maximum 20 acceptance criteria allowed.");

            RuleForEach(x => x.AcceptanceCriteria)
                .SetValidator(new CreateAcceptanceCriterionDtoValidator())
                .When(x => x.AcceptanceCriteria != null);
        }
    }

    public class CreateChecklistItemDtoValidator : AbstractValidator<CreateChecklistItemDto>
    {
        public CreateChecklistItemDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.Description)
                .MaximumLength(2000);
        }
    }

    public class CreateAcceptanceCriterionDtoValidator : AbstractValidator<CreateAcceptanceCriterionDto>
    {
        public CreateAcceptanceCriterionDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(x => x.GivenContext)
                .MaximumLength(500);

            RuleFor(x => x.WhenAction)
                .MaximumLength(500);

            RuleFor(x => x.ThenOutcome)
                .MaximumLength(500);
        }
    }
}