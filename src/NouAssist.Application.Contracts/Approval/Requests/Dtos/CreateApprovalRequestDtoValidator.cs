using FluentValidation;

namespace NouAssist.Approval.Requests.Dtos
{
    public class CreateApprovalRequestDtoValidator : AbstractValidator<CreateApprovalRequestDto>
    {
        public CreateApprovalRequestDtoValidator()
        {
            RuleFor(x => x.WorkflowId)
                .NotEmpty();

            RuleFor(x => x.EntityType)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.EntityId)
                .NotEmpty();

            RuleFor(x => x.Title)
                .MaximumLength(500);

            RuleFor(x => x.Description)
                .MaximumLength(2000);

            RuleFor(x => x.Priority)
                .InclusiveBetween(0, 10);
        }
    }
}
