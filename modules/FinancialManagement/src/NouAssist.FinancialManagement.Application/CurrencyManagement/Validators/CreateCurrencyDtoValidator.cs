using FluentValidation;
using NouAssist.FinancialManagement.CurrencyManagement.Dtos;

namespace NouAssist.FinancialManagement.CurrencyManagement.Validators
{
    public class CreateCurrencyDtoValidator : AbstractValidator<CreateCurrencyDto>
    {
        public CreateCurrencyDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .Length(3)
                .Matches("^[A-Z]{3}$")
                .WithMessage("Currency code must be 3 uppercase letters (ISO 4217)");

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(128);

            RuleFor(x => x.Symbol)
                .NotEmpty()
                .MaximumLength(10);

            RuleFor(x => x.DecimalPlaces)
                .InclusiveBetween(0, 8);
        }
    }
}
