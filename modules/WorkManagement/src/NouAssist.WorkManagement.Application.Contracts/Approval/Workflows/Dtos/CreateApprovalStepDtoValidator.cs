using FluentValidation;

namespace NouAssist.Approval.Workflows.Dtos
{
    public class CreateApprovalStepDtoValidator : AbstractValidator<CreateApprovalStepDto>
    {
        public CreateApprovalStepDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.OrderIndex)
                .GreaterThan(0);

            RuleFor(x => x.Requirement)
                .IsInEnum();

            RuleFor(x => x)
                .Must(HaveAtLeastOneApprover)
                .WithMessage("At least one approver (user, role, or team) must be specified");
        }

        private bool HaveAtLeastOneApprover(CreateApprovalStepDto dto)
        {
            return (dto.ApproverUserIds != null && dto.ApproverUserIds.Count > 0) ||
                   (dto.ApproverRoleIds != null && dto.ApproverRoleIds.Count > 0) ||
                   (dto.ApproverTeamIds != null && dto.ApproverTeamIds.Count > 0);
        }
    }
}
