using FluentValidation;
using NouAssist.FinancialManagement.InvestmentManagement.Portfolios.Dtos;

namespace NouAssist.FinancialManagement.InvestmentManagement.Portfolios
{
    public class BuyHoldingDtoValidator : AbstractValidator<BuyHoldingDto>
    {
        public BuyHoldingDtoValidator()
        {
            RuleFor(x => x.Symbol)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero");

            RuleFor(x => x.Fees)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Fees cannot be negative");

            RuleFor(x => x.TransactionDate)
                .LessThanOrEqualTo(System.DateTime.UtcNow)
                .WithMessage("Transaction date cannot be in the future");
        }
    }
}
