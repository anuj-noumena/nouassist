using FluentValidation;

namespace NouAssist.Approval.Workflows.Dtos
{
    public class CreateApprovalWorkflowDtoValidator : AbstractValidator<CreateApprovalWorkflowDto>
    {
        public CreateApprovalWorkflowDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(2000);

            RuleFor(x => x.WorkflowType)
                .IsInEnum();

            RuleFor(x => x.Strategy)
                .IsInEnum();
        }
    }
}
