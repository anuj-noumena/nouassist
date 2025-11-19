using FluentValidation;
using NouAssist.FinancialManagement.InvestmentManagement.Portfolios.Dtos;

namespace NouAssist.FinancialManagement.InvestmentManagement.Portfolios
{
    public class CreatePortfolioDtoValidator : AbstractValidator<CreatePortfolioDto>
    {
        public CreatePortfolioDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(1000);

            RuleFor(x => x.RiskTolerance)
                .IsInEnum();

            RuleFor(x => x.InitialCashDeposit)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Initial cash deposit cannot be negative");
        }
    }
}
