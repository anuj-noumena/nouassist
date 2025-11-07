using FluentValidation;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows.Requests.Dtos
{
    public class MakeApprovalDecisionDtoValidator : AbstractValidator<MakeApprovalDecisionDto>
    {
        public MakeApprovalDecisionDtoValidator()
        {
            RuleFor(x => x.Action)
                .IsInEnum();

            RuleFor(x => x.Comments)
                .MaximumLength(2000);
        }
    }
}
