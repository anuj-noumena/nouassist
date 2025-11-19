using NouAssist.FinancialManagement.AccountsPayable.Dtos;

namespace NouAssist.FinancialManagement.AccountsPayable.Validators
{
    public class CreateBillDtoValidator : AbstractValidator<CreateBillDto>
    {
        public CreateBillDtoValidator()
        {
            RuleFor(x => x.BillNumber)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.VendorId)
                .NotEmpty();

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero");

            RuleFor(x => x.IssueDate)
                .NotEmpty();

            RuleFor(x => x.DueDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(x => x.IssueDate)
                .WithMessage("Due date must be on or after issue date");

            RuleFor(x => x.CategoryId)
                .NotEmpty();
        }
    }
}
