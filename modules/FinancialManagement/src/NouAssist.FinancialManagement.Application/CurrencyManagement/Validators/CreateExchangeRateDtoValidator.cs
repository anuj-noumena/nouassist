using System;
using NouAssist.FinancialManagement.CurrencyManagement.Dtos;

namespace NouAssist.FinancialManagement.CurrencyManagement.Validators
{
    public class CreateExchangeRateDtoValidator : AbstractValidator<CreateExchangeRateDto>
    {
        public CreateExchangeRateDtoValidator()
        {
            RuleFor(x => x.BaseCurrency)
                .NotEmpty()
                .Length(3)
                .Matches("^[A-Z]{3}$");

            RuleFor(x => x.TargetCurrency)
                .NotEmpty()
                .Length(3)
                .Matches("^[A-Z]{3}$");

            RuleFor(x => x)
                .Must(x => x.BaseCurrency != x.TargetCurrency)
                .WithMessage("Base currency and target currency must be different");

            RuleFor(x => x.Rate)
                .GreaterThan(0)
                .WithMessage("Exchange rate must be positive");

            RuleFor(x => x.EffectiveDate)
                .LessThanOrEqualTo(DateTime.Today.AddDays(1))
                .WithMessage("Effective date cannot be more than 1 day in the future");
        }
    }
}
