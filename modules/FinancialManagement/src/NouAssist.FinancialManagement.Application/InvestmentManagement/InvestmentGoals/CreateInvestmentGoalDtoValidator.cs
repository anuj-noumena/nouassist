using FluentValidation;
using NouAssist.FinancialManagement.InvestmentManagement.InvestmentGoals.Dtos;

namespace NouAssist.FinancialManagement.InvestmentManagement.InvestmentGoals
{
    public class CreateInvestmentGoalDtoValidator : AbstractValidator<CreateInvestmentGoalDto>
    {
        public CreateInvestmentGoalDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(x => x.TargetAmount)
                .GreaterThan(0)
                .WithMessage("Target amount must be greater than zero");

            RuleFor(x => x.TargetDate)
                .GreaterThan(System.DateTime.UtcNow)
                .WithMessage("Target date must be in the future");
        }
    }
}
