using FluentValidation;
using NouAssist.FinancialManagement.Application.Contracts.AccountsReceivable.Dtos;

namespace NouAssist.FinancialManagement.Application.AccountsReceivable.Validators
{
    public class CreateInvoiceDtoValidator : AbstractValidator<CreateInvoiceDto>
    {
        public CreateInvoiceDtoValidator()
        {
            RuleFor(x => x.InvoiceNumber)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.CustomerId)
                .NotEmpty();

            RuleFor(x => x.IssueDate)
                .NotEmpty();

            RuleFor(x => x.DueDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(x => x.IssueDate)
                .WithMessage("Due date must be on or after issue date");

            RuleFor(x => x.LineItems)
                .NotEmpty()
                .WithMessage("Invoice must have at least one line item");

            RuleForEach(x => x.LineItems)
                .SetValidator(new CreateInvoiceLineItemDtoValidator());
        }
    }

    public class CreateInvoiceLineItemDtoValidator : AbstractValidator<CreateInvoiceLineItemDto>
    {
        public CreateInvoiceLineItemDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.Quantity)
                .GreaterThan(0);

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0);
        }
    }
}
