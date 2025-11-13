using FluentValidation;

namespace NouAssist.FinancialManagement.AccountsPayable.Dtos
{
    public class CreateBillDtoValidator : AbstractValidator<CreateBillDto>
    {
        public CreateBillDtoValidator()
        {
            RuleFor(x => x.BillNumber)
                .NotEmpty()
                .WithMessage("Bill number is required")
                .MaximumLength(50);

            RuleFor(x => x.VendorId)
                .NotEmpty()
                .WithMessage("Vendor is required");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be positive");

            RuleFor(x => x.IssueDate)
                .NotEmpty()
                .WithMessage("Issue date is required");

            RuleFor(x => x.DueDate)
                .NotEmpty()
                .WithMessage("Due date is required")
                .GreaterThanOrEqualTo(x => x.IssueDate)
                .WithMessage("Due date must be on or after issue date");

            RuleFor(x => x.LineItems)
                .NotEmpty()
                .WithMessage("At least one line item is required");
        }
    }
}
