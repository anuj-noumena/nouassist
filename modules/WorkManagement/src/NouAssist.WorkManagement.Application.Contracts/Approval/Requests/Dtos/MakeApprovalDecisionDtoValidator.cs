using FluentValidation;

namespace NouAssist.WorkManagement.Approval.Requests.Dtos
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
