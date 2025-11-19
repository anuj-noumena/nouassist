using FluentValidation;
using NouAssist.FinancialManagement.DebtManagement.PayoffPlans.Dtos;

namespace NouAssist.FinancialManagement.DebtManagement.PayoffPlans
{
    public class CreateDebtPayoffPlanDtoValidator : AbstractValidator<CreateDebtPayoffPlanDto>
    {
        public CreateDebtPayoffPlanDtoValidator()
        {
            RuleFor(x => x.PlanName)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.MonthlyPaymentAmount)
                .GreaterThan(0)
                .WithMessage("Monthly payment amount must be greater than zero");

            RuleFor(x => x.DebtAccountIds)
                .NotEmpty()
                .WithMessage("At least one debt account must be included in the plan");
        }
    }
}
