using System;
using NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos;

namespace NouAssist.WorkManagement.TaskManagement.TaskInstances.Validators
{
    public class DelegateTaskDtoValidator : AbstractValidator<DelegateTaskDto>
    {
        public DelegateTaskDtoValidator()
        {
            RuleFor(x => x.DelegatedTo)
                .NotEmpty();

            RuleFor(x => x.Note)
                .MaximumLength(2000);

            RuleFor(x => x.DueBy)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.DueBy.HasValue)
                .WithMessage("Due date must be in the future.");

            RuleFor(x => x.FollowupAt)
                .NotEmpty()
                .When(x => x.FollowupRequired)
                .WithMessage("Follow-up date is required when follow-up is enabled.");

            RuleFor(x => x.FollowupAt)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.FollowupAt.HasValue)
                .WithMessage("Follow-up date must be in the future.");
        }
    }
}
