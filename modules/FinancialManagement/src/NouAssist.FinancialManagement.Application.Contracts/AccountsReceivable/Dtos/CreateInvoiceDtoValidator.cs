using FluentValidation;

namespace NouAssist.FinancialManagement.AccountsReceivable.Dtos
{
    public class CreateInvoiceDtoValidator : AbstractValidator<CreateInvoiceDto>
    {
        public CreateInvoiceDtoValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("Customer is required");

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

            RuleForEach(x => x.LineItems)
                .Must(l => l.Quantity > 0 && l.UnitPrice > 0)
                .WithMessage("Quantity and unit price must be positive");
        }
    }
}
