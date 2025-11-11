using System;
using NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos;

namespace NouAssist.WorkManagement.TaskManagement.TaskInstances.Validators
{
    public class CreateTaskInstanceDtoValidator : AbstractValidator<CreateTaskInstanceDto>
    {
        public CreateTaskInstanceDtoValidator()
        {
            RuleFor(x => x.TaskId)
                .NotEmpty();

            RuleFor(x => x.ProjectId)
                .NotEmpty();

            RuleFor(x => x.WorkflowStatusId)
                .NotEmpty();

            RuleFor(x => x.Importance)
                .InclusiveBetween(0.0, 1.0)
                .When(x => x.Importance.HasValue);

            RuleFor(x => x.Urgency)
                .InclusiveBetween(0.0, 1.0)
                .When(x => x.Urgency.HasValue);

            RuleFor(x => x.PlannedDueDate)
                .GreaterThan(x => x.PlannedStartDate ?? DateTime.MinValue)
                .When(x => x.PlannedStartDate.HasValue && x.PlannedDueDate.HasValue)
                .WithMessage("Planned due date must be after planned start date.");
        }
    }
}
