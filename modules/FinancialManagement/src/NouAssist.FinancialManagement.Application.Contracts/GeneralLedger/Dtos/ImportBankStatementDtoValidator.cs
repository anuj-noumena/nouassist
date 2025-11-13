using FluentValidation;

namespace NouAssist.FinancialManagement.GeneralLedger.Dtos
{
    public class ImportBankStatementDtoValidator : AbstractValidator<ImportBankStatementDto>
    {
        public ImportBankStatementDtoValidator()
        {
            RuleFor(x => x.BankAccountId)
                .NotEmpty()
                .WithMessage("Bank account is required");

            RuleFor(x => x.StatementDate)
                .NotEmpty()
                .WithMessage("Statement date is required");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Start date is required");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("End date is required")
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after start date");

            RuleFor(x => x.Currency)
                .NotEmpty()
                .WithMessage("Currency is required")
                .Length(3)
                .WithMessage("Currency must be 3 characters (ISO 4217)");

            RuleFor(x => x.Lines)
                .NotEmpty()
                .WithMessage("Statement must contain at least one transaction");

            RuleForEach(x => x.Lines).SetValidator(new ImportBankStatementLineDtoValidator());
        }
    }

    public class ImportBankStatementLineDtoValidator : AbstractValidator<ImportBankStatementLineDto>
    {
        public ImportBankStatementLineDtoValidator()
        {
            RuleFor(x => x.TransactionDate)
                .NotEmpty()
                .WithMessage("Transaction date is required");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required")
                .MaximumLength(500);

            RuleFor(x => x.Amount)
                .NotEqual(0)
                .WithMessage("Amount cannot be zero");
        }
    }
}
