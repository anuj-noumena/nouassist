using NouAssist.FinancialManagement.TaxManagement.TaxReturns.Dtos;

namespace NouAssist.FinancialManagement.TaxManagement.TaxReturns
{
    public class AddTaxDeductionDtoValidator : AbstractValidator<AddTaxDeductionDto>
    {
        public AddTaxDeductionDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.ClaimedAmount)
                .GreaterThan(0)
                .WithMessage("Claimed amount must be greater than zero");
        }
    }
}
