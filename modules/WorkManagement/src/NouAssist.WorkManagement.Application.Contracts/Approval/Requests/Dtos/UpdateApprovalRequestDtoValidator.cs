using FluentValidation;

namespace NouAssist.WorkManagement.Approval.Requests.Dtos
{
    public class UpdateApprovalRequestDtoValidator : AbstractValidator<UpdateApprovalRequestDto>
    {
        public UpdateApprovalRequestDtoValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(500);

            RuleFor(x => x.Description)
                .MaximumLength(2000);

            RuleFor(x => x.Priority)
                .InclusiveBetween(0, 10);

            When(x => x.DueDate.HasValue, () =>
            {
                RuleFor(x => x.DueDate.Value)
                    .GreaterThan(System.DateTime.UtcNow)
                    .WithMessage("Due date must be in the future");
            });

            When(x => x.SlaDeadline.HasValue, () =>
            {
                RuleFor(x => x.SlaDeadline.Value)
                    .GreaterThan(System.DateTime.UtcNow)
                    .WithMessage("SLA deadline must be in the future");
            });
        }
    }
}