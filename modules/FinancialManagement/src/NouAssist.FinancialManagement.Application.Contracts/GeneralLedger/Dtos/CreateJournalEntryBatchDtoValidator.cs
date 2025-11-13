using FluentValidation;

namespace NouAssist.FinancialManagement.GeneralLedger.Dtos
{
    public class CreateJournalEntryBatchDtoValidator : AbstractValidator<CreateJournalEntryBatchDto>
    {
        public CreateJournalEntryBatchDtoValidator()
        {
            RuleFor(x => x.PostingDate)
                .NotEmpty()
                .WithMessage("Posting date is required");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required")
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.Lines)
                .NotEmpty()
                .WithMessage("At least one journal line is required")
                .Must(lines => lines.Count >= 2)
                .WithMessage("Journal entry must have at least 2 lines");

            RuleForEach(x => x.Lines)
                .SetValidator(new CreateJournalEntryLineDtoValidator());
        }
    }

    public class CreateJournalEntryLineDtoValidator : AbstractValidator<CreateJournalEntryLineDto>
    {
        public CreateJournalEntryLineDtoValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage("Account is required");

            RuleFor(x => x.Memo)
                .NotEmpty()
                .WithMessage("Memo is required")
                .MaximumLength(500);

            RuleFor(x => x)
                .Must(HaveEitherDebitOrCredit)
                .WithMessage("Line must have either a debit or credit amount, but not both");
        }

        private bool HaveEitherDebitOrCredit(CreateJournalEntryLineDto dto)
        {
            var hasDebit = dto.DebitAmount.HasValue && dto.DebitAmount.Value > 0;
            var hasCredit = dto.CreditAmount.HasValue && dto.CreditAmount.Value > 0;
            
            return (hasDebit && !hasCredit) || (!hasDebit && hasCredit);
        }
    }
}
