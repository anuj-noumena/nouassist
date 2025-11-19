using FluentValidation;
using NouAssist.FinancialManagement.DebtManagement.DebtAccounts.Dtos;

namespace NouAssist.FinancialManagement.DebtManagement.DebtAccounts
{
    public class CreateDebtAccountDtoValidator : AbstractValidator<CreateDebtAccountDto>
    {
        public CreateDebtAccountDtoValidator()
        {
            RuleFor(x => x.DebtName)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.OriginalAmount)
                .GreaterThan(0)
                .WithMessage("Original amount must be greater than zero");

            RuleFor(x => x.CurrentBalance)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Current balance cannot be negative");

            RuleFor(x => x.InterestRate)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Interest rate cannot be negative");

            RuleFor(x => x.MinimumPayment)
                .GreaterThan(0)
                .WithMessage("Minimum payment must be greater than zero");

            RuleFor(x => x.MaturityDate)
                .GreaterThan(x => x.StartDate)
                .When(x => x.StartDate.HasValue && x.MaturityDate.HasValue)
                .WithMessage("Maturity date must be after start date");
        }
    }
}
