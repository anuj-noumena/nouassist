using FluentValidation;
using NouAssist.FinancialManagement.Application.Contracts.GeneralLedger.Dtos;

namespace NouAssist.FinancialManagement.Application.GeneralLedger.Validators
{
    public class CreateJournalEntryBatchDtoValidator : AbstractValidator<CreateJournalEntryBatchDto>
    {
        public CreateJournalEntryBatchDtoValidator()
        {
            RuleFor(x => x.PostingDate)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.Today.AddDays(1))
                .WithMessage("Posting date cannot be more than 1 day in the future");

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.Lines)
                .NotEmpty()
                .WithMessage("Journal entry must have at least 2 lines")
                .Must(lines => lines.Count >= 2)
                .WithMessage("Journal entry must have at least 2 lines");

            RuleForEach(x => x.Lines)
                .SetValidator(new CreateJournalEntryLineDtoValidator());

            RuleFor(x => x.Lines)
                .Must(BeBalanced)
                .WithMessage("Journal entry must be balanced (total debits must equal total credits)");
        }

        private bool BeBalanced(List<CreateJournalEntryLineDto> lines)
        {
            var totalDebits = lines.Sum(l => l.DebitAmount ?? 0);
            var totalCredits = lines.Sum(l => l.CreditAmount ?? 0);
            return Math.Abs(totalDebits - totalCredits) < 0.01m;
        }
    }

    public class CreateJournalEntryLineDtoValidator : AbstractValidator<CreateJournalEntryLineDto>
    {
        public CreateJournalEntryLineDtoValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty();

            RuleFor(x => x)
                .Must(x => (x.DebitAmount.HasValue && !x.CreditAmount.HasValue) ||
                          (!x.DebitAmount.HasValue && x.CreditAmount.HasValue))
                .WithMessage("Line must have either debit or credit amount, but not both");

            RuleFor(x => x.Memo)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}
