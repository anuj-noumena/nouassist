using FluentValidation;
using NouAssist.TaxManagement.TaxReturns.Dtos;

namespace NouAssist.TaxManagement.TaxReturns
{
    public class CreateTaxReturnDtoValidator : AbstractValidator<CreateTaxReturnDto>
    {
        public CreateTaxReturnDtoValidator()
        {
            RuleFor(x => x.TaxYear)
                .InclusiveBetween(2000, 2100)
                .WithMessage("Tax year must be between 2000 and 2100");

            RuleFor(x => x.FilingStatus)
                .IsInEnum()
                .WithMessage("Invalid filing status");
        }
    }
}
