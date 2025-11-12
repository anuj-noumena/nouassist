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

            RuleFor(x => x.EstimateMinutes)
                .GreaterThan(0)
                .When(x => x.EstimateMinutes.HasValue);

            RuleFor(x => x.ActionSteps)
                .Must(steps => steps == null || steps.Count <= 50)
                .WithMessage("Maximum 50 action steps allowed.");

            RuleForEach(x => x.ActionSteps)
                .SetValidator(new CreateTaskActionStepDtoValidator())
                .When(x => x.ActionSteps != null);

            RuleFor(x => x.AcceptanceCriteria)
                .Must(criteria => criteria == null || criteria.Count <= 20)
                .WithMessage("Maximum 20 acceptance criteria allowed.");

            RuleForEach(x => x.AcceptanceCriteria)
                .SetValidator(new CreateAcceptanceCriterionDtoValidator())
                .When(x => x.AcceptanceCriteria != null);
        }
    }

    public class CreateTaskActionStepDtoValidator : AbstractValidator<CreateTaskActionStepDto>
    {
        public CreateTaskActionStepDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.Notes)
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
