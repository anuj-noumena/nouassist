using FluentValidation;
using NouAssist.FinancialManagement.CreditScoreMonitoring.CreditReports.Dtos;

namespace NouAssist.FinancialManagement.CreditScoreMonitoring.CreditReports
{
    public class ImportCreditReportDtoValidator : AbstractValidator<ImportCreditReportDto>
    {
        public ImportCreditReportDtoValidator()
        {
            RuleFor(x => x.Bureau)
                .IsInEnum()
                .WithMessage("Invalid credit bureau");

            RuleFor(x => x.ReportDate)
                .NotEmpty()
                .LessThanOrEqualTo(System.DateTime.UtcNow)
                .WithMessage("Report date cannot be in the future");

            RuleFor(x => x.CreditScore)
                .InclusiveBetween(300, 850)
                .WithMessage("Credit score must be between 300 and 850");

            RuleFor(x => x.Accounts)
                .NotEmpty()
                .WithMessage("At least one account must be provided");
        }
    }
}
